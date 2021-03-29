namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;

    /// <summary>
    /// Delete page command.
    /// </summary>
    public class DeletePageCommand
    {
        private readonly IPageRepository pageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletePageCommand"/> class.
        /// </summary>
        /// <param name="pageRepository">The page repository.</param>
        public DeletePageCommand(IPageRepository pageRepository) =>
            this.pageRepository = pageRepository;

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(ulong pageId, CancellationToken cancellationToken)
        {
            var filters = new Models.PageOptionFilter { PageId = pageId };
            var page = await this.pageRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (page is null || !page.Any())
            {
                return new NotFoundResult();
            }

            var item = page.First();
            await this.pageRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
