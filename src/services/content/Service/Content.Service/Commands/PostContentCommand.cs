namespace Content.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Content.Service.Constants;
    using Content.Service.Repositories;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Post content command.
    /// </summary>
    public class PostContentCommand
    {
        private readonly IContentRepository contentRepository;
        private readonly IMapper<Models.Content, Content> contentToContentMapper;
        private readonly IMapper<SaveContent, Models.Content> saveContentToContentMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostContentCommand"/> class.
        /// </summary>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="contentToContentMapper">The content to content mapper.</param>
        /// <param name="saveContentToContentMapper">The save content to content mapper.</param>
        public PostContentCommand(
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
        /// <param name="saveContent">The save content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SaveContent saveContent, CancellationToken cancellationToken)
        {
            var content = this.saveContentToContentMapper.Map(saveContent);
            content = await this.contentRepository.AddAsync(content, cancellationToken).ConfigureAwait(false);
            var contentViewModel = this.contentToContentMapper.Map(content);

            var filters = new Models.ContentOptionFilter { ContentId = contentViewModel.ContentId };
            return new CreatedAtRouteResult(
                ContentControllerRoute.GetContent,
                filters,
                contentViewModel);
        }
    }
}
