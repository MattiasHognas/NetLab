namespace Workspace.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Constants;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Post workspace command.
    /// </summary>
    public class PostWorkspaceCommand
    {
        private readonly IWorkspaceRepository workspaceRepository;
        private readonly IMapper<Models.Workspace, Workspace> workspaceToWorkspaceMapper;
        private readonly IMapper<SaveWorkspace, Models.Workspace> saveWorkspaceToWorkspaceMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostWorkspaceCommand"/> class.
        /// </summary>
        /// <param name="workspaceRepository">The workspace repository.</param>
        /// <param name="workspaceToWorkspaceMapper">The workspace to workspace mapper.</param>
        /// <param name="saveWorkspaceToWorkspaceMapper">The save workspace to workspace mapper.</param>
        public PostWorkspaceCommand(
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
        /// <param name="saveWorkspace">The save workspace.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SaveWorkspace saveWorkspace, CancellationToken cancellationToken)
        {
            var workspace = this.saveWorkspaceToWorkspaceMapper.Map(saveWorkspace);
            workspace = await this.workspaceRepository.AddAsync(workspace, cancellationToken).ConfigureAwait(false);
            var workspaceViewModel = this.workspaceToWorkspaceMapper.Map(workspace);

            var filters = new Models.WorkspaceOptionFilter { WorkspaceId = workspaceViewModel.WorkspaceId };
            return new CreatedAtRouteResult(
                WorkspaceControllerRoute.GetWorkspace,
                filters,
                workspaceViewModel);
        }
    }
}
