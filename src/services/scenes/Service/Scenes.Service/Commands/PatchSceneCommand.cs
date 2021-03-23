namespace Scenes.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Repositories;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class PatchSceneCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<Models.Scene, SaveScene> sceneToSaveSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

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

        public async Task<IActionResult> ExecuteAsync(
            int sceneId,
            JsonPatchDocument<SaveScene> patch,
            CancellationToken cancellationToken)
        {
            var scene = await this.sceneRepository.GetAsync(sceneId, cancellationToken).ConfigureAwait(false);
            if (scene is null)
            {
                return new NotFoundResult();
            }

            var saveScene = this.sceneToSaveSceneMapper.Map(scene);
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

            this.saveSceneToSceneMapper.Map(saveScene, scene);
            await this.sceneRepository.UpdateAsync(scene, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(scene);

            return new OkObjectResult(sceneViewModel);
        }
    }
}
