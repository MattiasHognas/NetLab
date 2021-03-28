namespace Workspace.Service.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Page repository.
    /// </summary>
    public interface IPageRepository
    {
        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        Task<Page> AddAsync(Page page, CancellationToken cancellationToken);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        Task DeleteAsync(Page page, CancellationToken cancellationToken);

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="pageOptionFilter">The page option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of page.</returns>
        Task<List<Page>> GetAsync(PageOptionFilter pageOptionFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        Task<Page> UpdateAsync(Page page, CancellationToken cancellationToken);
    }
}
