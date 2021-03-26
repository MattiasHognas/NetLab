namespace Workspace.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Workspace.Service.Repositories;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Patch workspace command.
    /// </summary>
    public class PatchWorkspaceCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly IWorkspaceRepository workspaceRepository;
        private readonly IMapper<Models.Workspace, Workspace> workspaceToWorkspaceMapper;
        private readonly IMapper<Models.Workspace, SaveWorkspace> workspaceToSaveWorkspaceMapper;
        private readonly IMapper<SaveWorkspace, Models.Workspace> saveWorkspaceToWorkspaceMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchWorkspaceCommand"/> class.
        /// </summary>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="objectModelValidator">The object model validator.</param>
        /// <param name="workspaceRepository">The workspace repository.</param>
        /// <param name="workspaceToWorkspaceMapper">The workspace to workspace mapper.</param>
        /// <param name="workspaceToSaveWorkspaceMapper">The workspace to save workspace mapper.</param>
        /// <param name="saveWorkspaceToWorkspaceMapper">The save workspace to workspace mapper.</param>
        public PatchWorkspaceCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            IWorkspaceRepository workspaceRepository,
            IMapper<Models.Workspace, Workspace> workspaceToWorkspaceMapper,
            IMapper<Models.Workspace, SaveWorkspace> workspaceToSaveWorkspaceMapper,
            IMapper<SaveWorkspace, Models.Workspace> saveWorkspaceToWorkspaceMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.workspaceRepository = workspaceRepository;
            this.workspaceToWorkspaceMapper = workspaceToWorkspaceMapper;
            this.workspaceToSaveWorkspaceMapper = workspaceToSaveWorkspaceMapper;
            this.saveWorkspaceToWorkspaceMapper = saveWorkspaceToWorkspaceMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="workspaceId">The workspace id.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(
            int workspaceId,
            JsonPatchDocument<SaveWorkspace> patch,
            CancellationToken cancellationToken)
        {
            var filters = new Models.WorkspaceOptionFilter { WorkspaceId = workspaceId };
            var workspace = await this.workspaceRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (workspace is null || !workspace.Any())
            {
                return new NotFoundResult();
            }

            var item = workspace.First();
            var saveWorkspace = this.workspaceToSaveWorkspaceMapper.Map(item);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveWorkspace, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveWorkspace);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.saveWorkspaceToWorkspaceMapper.Map(saveWorkspace, item);
            await this.workspaceRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var workspaceViewModel = this.workspaceToWorkspaceMapper.Map(item);

            return new OkObjectResult(workspaceViewModel);
        }
    }
}
