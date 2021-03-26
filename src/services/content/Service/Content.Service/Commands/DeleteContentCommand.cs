namespace Content.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Content.Service.Repositories;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Delete content command.
    /// </summary>
    public class DeleteContentCommand
    {
        private readonly IContentRepository contentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteContentCommand"/> class.
        /// </summary>
        /// <param name="contentRepository">The content repository.</param>
        public DeleteContentCommand(IContentRepository contentRepository) =>
            this.contentRepository = contentRepository;

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int contentId, CancellationToken cancellationToken)
        {
            var filters = new Models.ContentOptionFilter { ContentId = contentId };
            var content = await this.contentRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (content is null || !content.Any())
            {
                return new NotFoundResult();
            }

            var item = content.First();
            await this.contentRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
