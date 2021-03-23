namespace Scenes.Service.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Constants;
    using Scenes.Service.Repositories;
    using Scenes.Service.ViewModels;
    using Boxed.AspNetCore;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;

    public class GetScenePageCommand
    {
        private const string LinkHttpHeaderName = "Link";
        private const int DefaultPageSize = 3;
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public GetScenePageCommand(
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneMapper,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.sceneRepository = sceneRepository;
            this.sceneMapper = sceneMapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        public async Task<IActionResult> ExecuteAsync(PageOptions pageOptions, CancellationToken cancellationToken)
        {
            if (pageOptions is null)
            {
                throw new ArgumentNullException(nameof(pageOptions));
            }

            pageOptions.First = !pageOptions.First.HasValue && !pageOptions.Last.HasValue ? DefaultPageSize : pageOptions.First;
            var createdAfter = Cursor.FromCursor<DateTimeOffset?>(pageOptions.After);
            var createdBefore = Cursor.FromCursor<DateTimeOffset?>(pageOptions.Before);

            var getScenesTask = this.GetScenesAsync(pageOptions.First, pageOptions.Last, createdAfter, createdBefore, cancellationToken);
            var getHasNextPageTask = this.GetHasNextPageAsync(pageOptions.First, createdAfter, createdBefore, cancellationToken);
            var getHasPreviousPageTask = this.GetHasPreviousPageAsync(pageOptions.Last, createdAfter, createdBefore, cancellationToken);
            var totalCountTask = this.sceneRepository.GetTotalCountAsync(cancellationToken);

            await Task.WhenAll(getScenesTask, getHasNextPageTask, getHasPreviousPageTask, totalCountTask).ConfigureAwait(false);
            var scenes = await getScenesTask.ConfigureAwait(false);
            var hasNextPage = await getHasNextPageTask.ConfigureAwait(false);
            var hasPreviousPage = await getHasPreviousPageTask.ConfigureAwait(false);
            var totalCount = await totalCountTask.ConfigureAwait(false);

            if (scenes is null)
            {
                return new NotFoundResult();
            }

            var (startCursor, endCursor) = Cursor.GetFirstAndLastCursor(scenes, x => x.Created);
            var sceneViewModels = this.sceneMapper.MapList(scenes);

            var httpContext = this.httpContextAccessor.HttpContext!;
            var connection = new Connection<Scene>()
            {
                PageInfo = new PageInfo()
                {
                    Count = sceneViewModels.Count,
                    HasNextPage = hasNextPage,
                    HasPreviousPage = hasPreviousPage,
                    NextPageUrl = hasNextPage ? new Uri(this.linkGenerator.GetUriByRouteValues(
                        httpContext,
                        ScenesControllerRoute.GetScenePage,
                        new PageOptions()
                        {
                            First = pageOptions.First ?? pageOptions.Last,
                            After = endCursor,
                        })!) : null,
                    PreviousPageUrl = hasPreviousPage ? new Uri(this.linkGenerator.GetUriByRouteValues(
                        httpContext,
                        ScenesControllerRoute.GetScenePage,
                        new PageOptions()
                        {
                            Last = pageOptions.First ?? pageOptions.Last,
                            Before = startCursor,
                        })!) : null,
                    FirstPageUrl = new Uri(this.linkGenerator.GetUriByRouteValues(
                        httpContext,
                        ScenesControllerRoute.GetScenePage,
                        new PageOptions()
                        {
                            First = pageOptions.First ?? pageOptions.Last,
                        })!),
                    LastPageUrl = new Uri(this.linkGenerator.GetUriByRouteValues(
                        httpContext,
                        ScenesControllerRoute.GetScenePage,
                        new PageOptions()
                        {
                            Last = pageOptions.First ?? pageOptions.Last,
                        })!),
                },
                TotalCount = totalCount,
            };
            connection.Items.AddRange(sceneViewModels);

            httpContext.Response.Headers.Add(
                LinkHttpHeaderName,
                connection.PageInfo.ToLinkHttpHeaderValue());

            return new OkObjectResult(connection);
        }

        private Task<List<Models.Scene>> GetScenesAsync(
            int? first,
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            Task<List<Models.Scene>> getScenesTask;
            if (first.HasValue)
            {
                getScenesTask = this.sceneRepository.GetScenesAsync(first, createdAfter, createdBefore, cancellationToken);
            }
            else
            {
                getScenesTask = this.sceneRepository.GetScenesReverseAsync(last, createdAfter, createdBefore, cancellationToken);
            }

            return getScenesTask;
        }

        private async Task<bool> GetHasNextPageAsync(
            int? first,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            if (first.HasValue)
            {
                return await this.sceneRepository
                    .GetHasNextPageAsync(first, createdAfter, cancellationToken)
                    .ConfigureAwait(false);
            }
            else if (createdBefore.HasValue)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> GetHasPreviousPageAsync(
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken)
        {
            if (last.HasValue)
            {
                return await this.sceneRepository
                    .GetHasPreviousPageAsync(last, createdBefore, cancellationToken)
                    .ConfigureAwait(false);
            }
            else if (createdAfter.HasValue)
            {
                return true;
            }

            return false;
        }
    }
}
