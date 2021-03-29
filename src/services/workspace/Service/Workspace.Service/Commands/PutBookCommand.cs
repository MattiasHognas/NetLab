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
    /// Put book command.
    /// </summary>
    public class PutBookCommand
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper<Models.Book, Book> bookToBookMapper;
        private readonly IMapper<SaveBook, Models.Book> saveBookToBookMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutBookCommand"/> class.
        /// </summary>
        /// <param name="bookRepository">The book repository.</param>
        /// <param name="bookToBookMapper">The book to book mapper.</param>
        /// <param name="saveBookToBookMapper">The save book to book mapper.</param>
        public PutBookCommand(
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
        /// <param name="bookId">The book id.</param>
        /// <param name="saveBook">The save book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A action result.</returns>
        public async Task<IActionResult> ExecuteAsync(ulong bookId, SaveBook saveBook, CancellationToken cancellationToken)
        {
            var filters = new Models.BookOptionFilter { BookId = bookId };
            var book = await this.bookRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (book is null || !book.Any())
            {
                return new NotFoundResult();
            }

            var item = book.First();
            this.saveBookToBookMapper.Map(saveBook, item);
            item = await this.bookRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var bookViewModel = this.bookToBookMapper.Map(item);

            return new OkObjectResult(bookViewModel);
        }
    }
}
