namespace Scene.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Scene.Service.Repositories;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Patch scene command.
    /// </summary>
    public class PatchSceneCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<Models.Scene, SaveScene> sceneToSaveSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PatchSceneCommand"/> class.
        /// </summary>
        /// <param name="actionContextAccessor">The action context accessor.</param>
        /// <param name="objectModelValidator">The object model validator.</param>
        /// <param name="sceneRepository">The scene repository.</param>
        /// <param name="sceneToSceneMapper">The scene to scene mapper.</param>
        /// <param name="sceneToSaveSceneMapper">The scene to save scene mapper.</param>
        /// <param name="saveSceneToSceneMapper">The save scene to scene mapper.</param>
        public PatchSceneCommand(
            IActionContextAccessor actionContextAccessor,
            IObjectModelValidator objectModelValidator,
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneToSceneMapper,
            IMapper<Models.Scene, SaveScene> sceneToSaveSceneMapper,
            IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper)
        {
            this.actionContextAccessor = actionContextAccessor;
            this.objectModelValidator = objectModelValidator;
            this.sceneRepository = sceneRepository;
            this.sceneToSceneMapper = sceneToSceneMapper;
            this.sceneToSaveSceneMapper = sceneToSaveSceneMapper;
            this.saveSceneToSceneMapper = saveSceneToSceneMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="sceneId">The scene id.</param>
        /// <param name="patch">The patch.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(
            int sceneId,
            JsonPatchDocument<SaveScene> patch,
            CancellationToken cancellationToken)
        {
            var filters = new Models.SceneOptionFilter { SceneId = sceneId };
            var scene = await this.sceneRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (scene is null || !scene.Any())
            {
                return new NotFoundResult();
            }

            var item = scene.First();
            var saveScene = this.sceneToSaveSceneMapper.Map(item);
            var modelState = this.actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveScene, modelState);
            this.objectModelValidator.Validate(
                this.actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveScene);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }

            this.saveSceneToSceneMapper.Map(saveScene, item);
            await this.sceneRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(item);

            return new OkObjectResult(sceneViewModel);
        }
    }
}
