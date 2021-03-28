namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Put page command.
    /// </summary>
    public class PutPageCommand
    {
        private readonly IPageRepository pageRepository;
        private readonly IMapper<Models.Page, Page> pageToPageMapper;
        private readonly IMapper<SavePage, Models.Page> savePageToPageMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutPageCommand"/> class.
        /// </summary>
        /// <param name="pageRepository">The page repository.</param>
        /// <param name="pageToPageMapper">The page to page mapper.</param>
        /// <param name="savePageToPageMapper">The save page to page mapper.</param>
        public PutPageCommand(
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
        /// <param name="pageId">The page id.</param>
        /// <param name="savePage">The save page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int pageId, SavePage savePage, CancellationToken cancellationToken)
        {
            var filters = new Models.PageOptionFilter { PageId = pageId };
            var page = await this.pageRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (page is null || !page.Any())
            {
                return new NotFoundResult();
            }

            var item = page.First();
            this.savePageToPageMapper.Map(savePage, item);
            item = await this.pageRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var pageViewModel = this.pageToPageMapper.Map(item);

            return new OkObjectResult(pageViewModel);
        }
    }
}
