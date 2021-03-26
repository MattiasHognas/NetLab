namespace Workspace.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Workspace repository.
    /// </summary>
    public class WorkspaceRepository : IWorkspaceRepository
    {
        private static readonly List<Workspace> Workspace = new()
        {
            new Workspace()
            {
                WorkspaceId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "A",
                Description = "A",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Workspace()
            {
                WorkspaceId = 2,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "B",
                Description = "B",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Workspace()
            {
                WorkspaceId = 3,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "C",
                Description = "C",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
        };

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A workspace.</returns>
        public Task<Workspace> AddAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            if (workspace is null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            Workspace.Add(workspace);
            workspace.WorkspaceId = Workspace.Max(x => x.WorkspaceId) + 1;
            return Task.FromResult(workspace);
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            if (Workspace.Contains(workspace))
            {
                Workspace.Remove(workspace);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="workspaceOptionFilter">The workspace option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of workspace.</returns>
        public Task<List<Workspace>> GetAsync(
            WorkspaceOptionFilter workspaceOptionFilter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Workspace
                .OrderBy(x => x.Created)
                .If(workspaceOptionFilter.WorkspaceId.HasValue, x => x.Where(y => y.WorkspaceId == workspaceOptionFilter.WorkspaceId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Workspace.Count);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A workspace.</returns>
        public Task<Workspace> UpdateAsync(Workspace workspace, CancellationToken cancellationToken)
        {
            if (workspace is null)
            {
                throw new ArgumentNullException(nameof(workspace));
            }

            var existingWorkspace = Workspace.First(x => x.WorkspaceId == workspace.WorkspaceId);
            existingWorkspace.Name = workspace.Name;
            existingWorkspace.Description = workspace.Description;
            return Task.FromResult(workspace);
        }
    }
}
