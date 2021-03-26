namespace Workspace.Service.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Get workspace command.
    /// </summary>
    public class GetWorkspaceCommand
    {
        private readonly IWorkspaceRepository workspaceRepository;
        private readonly IMapper<Models.Workspace, Workspace> workspaceMapper;
        private readonly IMapper<WorkspaceOptionFilter, Models.WorkspaceOptionFilter> workspaceOptionFilterMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetWorkspaceCommand"/> class.
        /// </summary>
        /// <param name="workspaceRepository">The workspace repository.</param>
        /// <param name="workspaceMapper">The workspace mapper.</param>
        /// <param name="workspaceOptionFilterMapper">The workspace option mapper.</param>
        public GetWorkspaceCommand(
            IWorkspaceRepository workspaceRepository,
            IMapper<Models.Workspace, Workspace> workspaceMapper,
            IMapper<WorkspaceOptionFilter, Models.WorkspaceOptionFilter> workspaceOptionFilterMapper)
        {
            this.workspaceRepository = workspaceRepository;
            this.workspaceMapper = workspaceMapper;
            this.workspaceOptionFilterMapper = workspaceOptionFilterMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="workspaceOptionFilter">The workspace option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(WorkspaceOptionFilter workspaceOptionFilter, CancellationToken cancellationToken)
        {
            if (workspaceOptionFilter is null)
            {
                throw new ArgumentNullException(nameof(workspaceOptionFilter));
            }

            var workspaceOptionFilterModel = this.workspaceOptionFilterMapper.Map(workspaceOptionFilter);
            var workspaces = await this.workspaceRepository.GetAsync(workspaceOptionFilterModel, cancellationToken).ConfigureAwait(false);

            if (workspaces is null || !workspaces.Any())
            {
                return new NotFoundResult();
            }

            var workspaceViewModels = this.workspaceMapper.MapList(workspaces);
            return new OkObjectResult(workspaceViewModels);
        }
    }
}
