namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;

    /// <summary>
    /// Delete book command.
    /// </summary>
    public class DeleteBookCommand
    {
        private readonly IBookRepository bookRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBookCommand"/> class.
        /// </summary>
        /// <param name="bookRepository">The book repository.</param>
        public DeleteBookCommand(IBookRepository bookRepository) =>
            this.bookRepository = bookRepository;

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(ulong bookId, CancellationToken cancellationToken)
        {
            var filters = new Models.BookOptionFilter { BookId = bookId };
            var book = await this.bookRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (book is null || !book.Any())
            {
                return new NotFoundResult();
            }

            var item = book.First();
            await this.bookRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
