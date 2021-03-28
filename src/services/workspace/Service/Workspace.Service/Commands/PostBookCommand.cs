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
    /// Post book command.
    /// </summary>
    public class PostBookCommand
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper<Models.Book, Book> bookToBookMapper;
        private readonly IMapper<SaveBook, Models.Book> saveBookToBookMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostBookCommand"/> class.
        /// </summary>
        /// <param name="bookRepository">The book repository.</param>
        /// <param name="bookToBookMapper">The book to book mapper.</param>
        /// <param name="saveBookToBookMapper">The save book to book mapper.</param>
        public PostBookCommand(
            IBookRepository bookRepository,
            IMapper<Models.Book, Book> bookToBookMapper,
            IMapper<SaveBook, Models.Book> saveBookToBookMapper)
        {
            this.bookRepository = bookRepository;
            this.bookToBookMapper = bookToBookMapper;
            this.saveBookToBookMapper = saveBookToBookMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="saveBook">The save book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SaveBook saveBook, CancellationToken cancellationToken)
        {
            var book = this.saveBookToBookMapper.Map(saveBook);
            book = await this.bookRepository.AddAsync(book, cancellationToken).ConfigureAwait(false);
            var bookViewModel = this.bookToBookMapper.Map(book);

            var filters = new Models.BookOptionFilter { BookId = bookViewModel.BookId };
            return new CreatedAtRouteResult(
                BookControllerRoute.GetBook,
                filters,
                bookViewModel);
        }
    }
}
