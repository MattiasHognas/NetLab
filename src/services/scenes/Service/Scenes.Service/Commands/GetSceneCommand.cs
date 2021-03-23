namespace Scenes.Service.Commands
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Repositories;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Net.Http.Headers;

    public class GetSceneCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneMapper;

        public GetSceneCommand(
            IActionContextAccessor actionContextAccessor,
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.sceneRepository = sceneRepository;
            this.sceneMapper = sceneMapper;
        }

        public async Task<IActionResult> ExecuteAsync(int sceneId, CancellationToken cancellationToken)
        {
            var scene = await this.sceneRepository.GetAsync(sceneId, cancellationToken).ConfigureAwait(false);
            if (scene is null)
            {
                return new NotFoundResult();
            }

            var httpContext = this.actionContextAccessor.ActionContext.HttpContext;
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                if (DateTimeOffset.TryParse(stringValues, out var modifiedSince) &&
                    (modifiedSince >= scene.Modified))
                {
                    return new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }

            var sceneViewModel = this.sceneMapper.Map(scene);
            httpContext.Response.Headers.Add(
                HeaderNames.LastModified,
                scene.Modified.ToString("R", CultureInfo.InvariantCulture));
            return new OkObjectResult(sceneViewModel);
        }
    }
}
