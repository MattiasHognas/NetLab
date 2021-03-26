namespace Content.Service.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Content.Service.Models;

    /// <summary>
    /// Content repository.
    /// </summary>
    public interface IContentRepository
    {
        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        Task<Content> AddAsync(Content content, CancellationToken cancellationToken);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        Task DeleteAsync(Content content, CancellationToken cancellationToken);

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="contentOptionFilter">The content option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of content.</returns>
        Task<List<Content>> GetAsync(ContentOptionFilter contentOptionFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        Task<Content> UpdateAsync(Content content, CancellationToken cancellationToken);
    }
}
