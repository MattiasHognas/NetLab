namespace Workspace.Service.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Book repository.
    /// </summary>
    public interface IBookRepository
    {
        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        Task<Book> AddAsync(Book book, CancellationToken cancellationToken);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        Task DeleteAsync(Book book, CancellationToken cancellationToken);

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="bookOptionFilter">The book option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of book.</returns>
        Task<List<Book>> GetAsync(BookOptionFilter bookOptionFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="book">The book.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A book.</returns>
        Task<Book> UpdateAsync(Book book, CancellationToken cancellationToken);
    }
}
