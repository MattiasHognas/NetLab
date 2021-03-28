namespace Workspace.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Constants;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Post page command.
    /// </summary>
    public class PostPageCommand
    {
        private readonly IPageRepository pageRepository;
        private readonly IMapper<Models.Page, Page> pageToPageMapper;
        private readonly IMapper<SavePage, Models.Page> savePageToPageMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostPageCommand"/> class.
        /// </summary>
        /// <param name="pageRepository">The page repository.</param>
        /// <param name="pageToPageMapper">The page to page mapper.</param>
        /// <param name="savePageToPageMapper">The save page to page mapper.</param>
        public PostPageCommand(
            IPageRepository pageRepository,
            IMapper<Models.Page, Page> pageToPageMapper,
            IMapper<SavePage, Models.Page> savePageToPageMapper)
        {
            this.pageRepository = pageRepository;
            this.pageToPageMapper = pageToPageMapper;
            this.savePageToPageMapper = savePageToPageMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="savePage">The save page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SavePage savePage, CancellationToken cancellationToken)
        {
            var page = this.savePageToPageMapper.Map(savePage);
            page = await this.pageRepository.AddAsync(page, cancellationToken).ConfigureAwait(false);
            var pageViewModel = this.pageToPageMapper.Map(page);

            var filters = new Models.PageOptionFilter { PageId = pageViewModel.PageId };
            return new CreatedAtRouteResult(
                PageControllerRoute.GetPage,
                filters,
                pageViewModel);
        }
    }
}
