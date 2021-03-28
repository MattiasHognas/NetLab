namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Patch page command.
    /// </summary>
    public class PatchPageCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly IPageRepository pageRepository;
        private readonly IMapper<Models.Page, Page> pageToPageMapper;
        private readonly IMapper<Models.Page, SavePage> pageToSavePageMapper;
        private readonly IMapper<SavePage, Models.Page> savePageToPageMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchPageCommand"/> class.
        /// </summary>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="objectModelValidator">The object model validator.</param>
        /// <param name="pageRepository">The page repository.</param>
        /// <param name="pageToPageMapper">The page to page mapper.</param>
        /// <param name="pageToSavePageMapper">The page to save page mapper.</param>
        /// <param name="savePageToPageMapper">The save page to page mapper.</param>
        public PatchPageCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            IPageRepository pageRepository,
            IMapper<Models.Page, Page> pageToPageMapper,
            IMapper<Models.Page, SavePage> pageToSavePageMapper,
            IMapper<SavePage, Models.Page> savePageToPageMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.pageRepository = pageRepository;
            this.pageToPageMapper = pageToPageMapper;
            this.pageToSavePageMapper = pageToSavePageMapper;
            this.savePageToPageMapper = savePageToPageMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="pageOptionFilter">The page option filter.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(
            PageOptionFilter pageOptionFilter,
            JsonPatchDocument<SavePage> patch,
            CancellationToken cancellationToken)
        {
            var filters = new Models.PageOptionFilter { PageId = pageOptionFilter.PageId, BookId = pageOptionFilter.BookId };
            var page = await this.pageRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (page is null || !page.Any())
            {
                return new NotFoundResult();
            }

            var item = page.First();
            var savePage = this.pageToSavePageMapper.Map(item);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(savePage, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: savePage);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.savePageToPageMapper.Map(savePage, item);
            await this.pageRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var pageViewModel = this.pageToPageMapper.Map(item);

            return new OkObjectResult(pageViewModel);
        }
    }
}
