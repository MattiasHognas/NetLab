namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Put workspace command.
    /// </summary>
    public class PutWorkspaceCommand
    {
        private readonly IWorkspaceRepository workspaceRepository;
        private readonly IMapper<Models.Workspace, Workspace> workspaceToWorkspaceMapper;
        private readonly IMapper<SaveWorkspace, Models.Workspace> saveWorkspaceToWorkspaceMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutWorkspaceCommand"/> class.
        /// </summary>
        /// <param name="workspaceRepository">The workspace repository.</param>
        /// <param name="workspaceToWorkspaceMapper">The workspace to workspace mapper.</param>
        /// <param name="saveWorkspaceToWorkspaceMapper">The save workspace to workspace mapper.</param>
        public PutWorkspaceCommand(
            IWorkspaceRepository workspaceRepository,
            IMapper<Models.Workspace, Workspace> workspaceToWorkspaceMapper,
            IMapper<SaveWorkspace, Models.Workspace> saveWorkspaceToWorkspaceMapper)
        {
            this.workspaceRepository = workspaceRepository;
            this.workspaceToWorkspaceMapper = workspaceToWorkspaceMapper;
            this.saveWorkspaceToWorkspaceMapper = saveWorkspaceToWorkspaceMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="workspaceId">The workspace id.</param>
        /// <param name="saveWorkspace">The save workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int workspaceId, SaveWorkspace saveWorkspace, CancellationToken cancellationToken)
        {
            var filters = new Models.WorkspaceOptionFilter { WorkspaceId = workspaceId };
            var workspace = await this.workspaceRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (workspace is null || !workspace.Any())
            {
                return new NotFoundResult();
            }

            var item = workspace.First();
            this.saveWorkspaceToWorkspaceMapper.Map(saveWorkspace, item);
            item = await this.workspaceRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var workspaceViewModel = this.workspaceToWorkspaceMapper.Map(item);

            return new OkObjectResult(workspaceViewModel);
        }
    }
}
