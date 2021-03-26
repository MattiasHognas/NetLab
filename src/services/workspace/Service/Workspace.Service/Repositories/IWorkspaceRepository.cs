namespace Workspace.Service.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Workspace repository.
    /// </summary>
    public interface IWorkspaceRepository
    {
        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A workspace.</returns>
        Task<Workspace> AddAsync(Workspace workspace, CancellationToken cancellationToken);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        Task DeleteAsync(Workspace workspace, CancellationToken cancellationToken);

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="workspaceOptionFilter">The workspace option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of workspace.</returns>
        Task<List<Workspace>> GetAsync(WorkspaceOptionFilter workspaceOptionFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A workspace.</returns>
        Task<Workspace> UpdateAsync(Workspace workspace, CancellationToken cancellationToken);
    }
}
