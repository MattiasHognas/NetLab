namespace Content.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Content.Service.Repositories;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    /// <summary>
    /// Patch content command.
    /// </summary>
    public class PatchContentCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly IContentRepository contentRepository;
        private readonly IMapper<Models.Content, Content> contentToContentMapper;
        private readonly IMapper<Models.Content, SaveContent> contentToSaveContentMapper;
        private readonly IMapper<SaveContent, Models.Content> saveContentToContentMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchContentCommand"/> class.
        /// </summary>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="objectModelValidator">The object model validator.</param>
        /// <param name="contentRepository">The content repository.</param>
        /// <param name="contentToContentMapper">The content to content mapper.</param>
        /// <param name="contentToSaveContentMapper">The content to save content mapper.</param>
        /// <param name="saveContentToContentMapper">The save content to content mapper.</param>
        public PatchContentCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            IContentRepository contentRepository,
            IMapper<Models.Content, Content> contentToContentMapper,
            IMapper<Models.Content, SaveContent> contentToSaveContentMapper,
            IMapper<SaveContent, Models.Content> saveContentToContentMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.contentRepository = contentRepository;
            this.contentToContentMapper = contentToContentMapper;
            this.contentToSaveContentMapper = contentToSaveContentMapper;
            this.saveContentToContentMapper = saveContentToContentMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="contentId">The content id.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(
            int contentId,
            JsonPatchDocument<SaveContent> patch,
            CancellationToken cancellationToken)
        {
            var filters = new Models.ContentOptionFilter { ContentId = contentId };
            var content = await this.contentRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (content is null || !content.Any())
            {
                return new NotFoundResult();
            }

            var item = content.First();
            var saveContent = this.contentToSaveContentMapper.Map(item);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveContent, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveContent);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.saveContentToContentMapper.Map(saveContent, item);
            await this.contentRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var contentViewModel = this.contentToContentMapper.Map(item);

            return new OkObjectResult(contentViewModel);
        }
    }
}
