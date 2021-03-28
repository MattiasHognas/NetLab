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
    /// Patch book command.
    /// </summary>
    public class PatchBookCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly IBookRepository bookRepository;
        private readonly IMapper<Models.Book, Book> bookToBookMapper;
        private readonly IMapper<Models.Book, SaveBook> bookToSaveBookMapper;
        private readonly IMapper<SaveBook, Models.Book> saveBookToBookMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchBookCommand"/> class.
        /// </summary>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="objectModelValidator">The object model validator.</param>
        /// <param name="bookRepository">The book repository.</param>
        /// <param name="bookToBookMapper">The book to book mapper.</param>
        /// <param name="bookToSaveBookMapper">The book to save book mapper.</param>
        /// <param name="saveBookToBookMapper">The save book to book mapper.</param>
        public PatchBookCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            IBookRepository bookRepository,
            IMapper<Models.Book, Book> bookToBookMapper,
            IMapper<Models.Book, SaveBook> bookToSaveBookMapper,
            IMapper<SaveBook, Models.Book> saveBookToBookMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.bookRepository = bookRepository;
            this.bookToBookMapper = bookToBookMapper;
            this.bookToSaveBookMapper = bookToSaveBookMapper;
            this.saveBookToBookMapper = saveBookToBookMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(
            int bookId,
            JsonPatchDocument<SaveBook> patch,
            CancellationToken cancellationToken)
        {
            var filters = new Models.BookOptionFilter { BookId = bookId };
            var book = await this.bookRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (book is null || !book.Any())
            {
                return new NotFoundResult();
            }

            var item = book.First();
            var saveBook = this.bookToSaveBookMapper.Map(item);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveBook, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveBook);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.saveBookToBookMapper.Map(saveBook, item);
            await this.bookRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var bookViewModel = this.bookToBookMapper.Map(item);

            return new OkObjectResult(bookViewModel);
        }
    }
}
