namespace Workspace.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Workspace.Service.Data;
    using Workspace.Service.Models;

    /// <summary>
    /// Book repository.
    /// </summary>
    public class BookRepository : IBookRepository
    {
        private readonly IDbContextFactory<WorkspaceContext> workspaceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookRepository"/> class.
        /// </summary>
        /// <param name="workspaceContextFactory">The content context factory.</param>
        public BookRepository(IDbContextFactory<WorkspaceContext> workspaceContextFactory) => this.workspaceContextFactory = workspaceContextFactory;

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        public Task<Book> AddAsync(Book book, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (book is null)
                {
                    throw new ArgumentNullException(nameof(book));
                }

                context.Books.Add(book);
                return Task.FromResult(book);
            }
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Book book, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (context.Books.Contains(book))
                {
                    context.Books.Remove(book);
                }

                return Task.CompletedTask;
            }
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
            Task.FromResult(this.workspaceContextFactory.CreateDbContext()
                .Books
                .OrderBy(x => x.ModifiedDate)
                .ThenBy(x => x.CreatedDate)
                .If(bookOptionFilter.BookId.HasValue, x => x.Where(y => y.BookId == bookOptionFilter.BookId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) =>
            Task.FromResult(this.workspaceContextFactory.CreateDbContext().Books.Count());

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        public Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (book is null)
                {
                    throw new ArgumentNullException(nameof(book));
                }

                var existingBook = context.Books.First(x => x.BookId == book.BookId);
                existingBook.Name = book.Name;
                existingBook.Description = book.Description;
                return Task.FromResult(book);
            }
        }
    }
}
