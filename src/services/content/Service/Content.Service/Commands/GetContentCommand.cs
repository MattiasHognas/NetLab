namespace Content.Service.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Content.Service.Repositories;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Get content command.
    /// </summary>
    public class GetContentCommand
    {
        private readonly IContentRepository contentRepository;
        private readonly IMapper<Models.Content, Content> contentMapper;
        private readonly IMapper<ContentOptionFilter, Models.ContentOptionFilter> contentOptionFilterMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetContentCommand"/> class.
        /// </summary>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="contentMapper">The content mapper.</param>
        /// <param name="contentOptionFilterMapper">The content option mapper.</param>
        public GetContentCommand(
            IContentRepository contentRepository,
            IMapper<Models.Content, Content> contentMapper,
            IMapper<ContentOptionFilter, Models.ContentOptionFilter> contentOptionFilterMapper)
        {
            this.contentRepository = contentRepository;
            this.contentMapper = contentMapper;
            this.contentOptionFilterMapper = contentOptionFilterMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="contentOptionFilter">The content option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(ContentOptionFilter contentOptionFilter, CancellationToken cancellationToken)
        {
            if (contentOptionFilter is null)
            {
                throw new ArgumentNullException(nameof(contentOptionFilter));
            }

            var contentOptionFilterModel = this.contentOptionFilterMapper.Map(contentOptionFilter);
            var contents = await this.contentRepository.GetAsync(contentOptionFilterModel, cancellationToken).ConfigureAwait(false);

            if (contents is null || !contents.Any())
            {
                return new NotFoundResult();
            }

            var contentViewModels = this.contentMapper.MapList(contents);
            return new OkObjectResult(contentViewModels);
        }
    }
}
