namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;

    /// <summary>
    /// Delete workspace command.
    /// </summary>
    public class DeleteWorkspaceCommand
    {
        private readonly IWorkspaceRepository workspaceRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteWorkspaceCommand"/> class.
        /// </summary>
        /// <param name="workspaceRepository">The workspace repository.</param>
        public DeleteWorkspaceCommand(IWorkspaceRepository workspaceRepository) =>
            this.workspaceRepository = workspaceRepository;

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="workspaceId">The workspace id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int workspaceId, CancellationToken cancellationToken)
        {
            var filters = new Models.WorkspaceOptionFilter { WorkspaceId = workspaceId };
            var workspace = await this.workspaceRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (workspace is null || !workspace.Any())
            {
                return new NotFoundResult();
            }

            var item = workspace.First();
            await this.workspaceRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
