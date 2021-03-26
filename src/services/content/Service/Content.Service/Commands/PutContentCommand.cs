namespace Content.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Content.Service.Repositories;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Put content command.
    /// </summary>
    public class PutContentCommand
    {
        private readonly IContentRepository contentRepository;
        private readonly IMapper<Models.Content, Content> contentToContentMapper;
        private readonly IMapper<SaveContent, Models.Content> saveContentToContentMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutContentCommand"/> class.
        /// </summary>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="contentToContentMapper">The content to content mapper.</param>
        /// <param name="saveContentToContentMapper">The save content to content mapper.</param>
        public PutContentCommand(
            IContentRepository contentRepository,
            IMapper<Models.Content, Content> contentToContentMapper,
            IMapper<SaveContent, Models.Content> saveContentToContentMapper)
        {
            this.contentRepository = contentRepository;
            this.contentToContentMapper = contentToContentMapper;
            this.saveContentToContentMapper = saveContentToContentMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="saveContent">The save content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int contentId, SaveContent saveContent, CancellationToken cancellationToken)
        {
            var filters = new Models.ContentOptionFilter { ContentId = contentId };
            var content = await this.contentRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (content is null || !content.Any())
            {
                return new NotFoundResult();
            }

            var item = content.First();
            this.saveContentToContentMapper.Map(saveContent, item);
            item = await this.contentRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var contentViewModel = this.contentToContentMapper.Map(item);

            return new OkObjectResult(contentViewModel);
        }
    }
}
