namespace Workspace.Service.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Get page command.
    /// </summary>
    public class GetPageCommand
    {
        private readonly IPageRepository pageRepository;
        private readonly IMapper<Models.Page, Page> pageMapper;
        private readonly IMapper<PageOptionFilter, Models.PageOptionFilter> pageOptionFilterMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetPageCommand"/> class.
        /// </summary>
        /// <param name="pageRepository">The page repository.</param>
        /// <param name="pageMapper">The page mapper.</param>
        /// <param name="pageOptionFilterMapper">The page option mapper.</param>
        public GetPageCommand(
            IPageRepository pageRepository,
            IMapper<Models.Page, Page> pageMapper,
            IMapper<PageOptionFilter, Models.PageOptionFilter> pageOptionFilterMapper)
        {
            this.pageRepository = pageRepository;
            this.pageMapper = pageMapper;
            this.pageOptionFilterMapper = pageOptionFilterMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="pageOptionFilter">The page option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(PageOptionFilter pageOptionFilter, CancellationToken cancellationToken)
        {
            if (pageOptionFilter is null)
            {
                throw new ArgumentNullException(nameof(pageOptionFilter));
            }

            var pageOptionFilterModel = this.pageOptionFilterMapper.Map(pageOptionFilter);
            var pages = await this.pageRepository.GetAsync(pageOptionFilterModel, cancellationToken).ConfigureAwait(false);

            if (pages is null || !pages.Any())
            {
                return new NotFoundResult();
            }

            var pageViewModels = this.pageMapper.MapList(pages);
            return new OkObjectResult(pageViewModels);
        }
    }
}
