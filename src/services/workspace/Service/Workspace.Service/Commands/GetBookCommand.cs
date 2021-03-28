namespace Workspace.Service.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Get book command.
    /// </summary>
    public class GetBookCommand
    {
        private readonly IBookRepository bookRepository;
        private readonly IMapper<Models.Book, Book> bookMapper;
        private readonly IMapper<BookOptionFilter, Models.BookOptionFilter> bookOptionFilterMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetBookCommand"/> class.
        /// </summary>
        /// <param name="bookRepository">The book repository.</param>
        /// <param name="bookMapper">The book mapper.</param>
        /// <param name="bookOptionFilterMapper">The book option mapper.</param>
        public GetBookCommand(
            IBookRepository bookRepository,
            IMapper<Models.Book, Book> bookMapper,
            IMapper<BookOptionFilter, Models.BookOptionFilter> bookOptionFilterMapper)
        {
            this.bookRepository = bookRepository;
            this.bookMapper = bookMapper;
            this.bookOptionFilterMapper = bookOptionFilterMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="bookOptionFilter">The book option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(BookOptionFilter bookOptionFilter, CancellationToken cancellationToken)
        {
            if (bookOptionFilter is null)
            {
                throw new ArgumentNullException(nameof(bookOptionFilter));
            }

            var bookOptionFilterModel = this.bookOptionFilterMapper.Map(bookOptionFilter);
            var books = await this.bookRepository.GetAsync(bookOptionFilterModel, cancellationToken).ConfigureAwait(false);

            if (books is null || !books.Any())
            {
                return new NotFoundResult();
            }

            var bookViewModels = this.bookMapper.MapList(books);
            return new OkObjectResult(bookViewModels);
        }
    }
}
