namespace Workspace.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Book repository.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private static readonly List<Book> Book = new()
        {
            new Book()
            {
                BookId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "A",
                Description = "A",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Book()
            {
                BookId = 2,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "B",
                Description = "B",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Book()
            {
                BookId = 3,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "C",
                Description = "C",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
        };

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        public Task<Book> AddAsync(Book book, CancellationToken cancellationToken)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            Book.Add(book);
            book.BookId = Book.Max(x => x.BookId) + 1;
            return Task.FromResult(book);
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Book book, CancellationToken cancellationToken)
        {
            if (Book.Contains(book))
            {
                Book.Remove(book);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="bookOptionFilter">The book option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of book.</returns>
        public Task<List<Book>> GetAsync(
            BookOptionFilter bookOptionFilter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Book
                .OrderBy(x => x.Created)
                .If(bookOptionFilter.BookId.HasValue, x => x.Where(y => y.BookId == bookOptionFilter.BookId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Book.Count);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            if (book is null)
            {
                throw new ArgumentNullException(nameof(book));
            }

            var existingBook = Book.First(x => x.BookId == book.BookId);
            existingBook.Name = book.Name;
            existingBook.Description = book.Description;
            return Task.FromResult(book);
        }
    }
}
