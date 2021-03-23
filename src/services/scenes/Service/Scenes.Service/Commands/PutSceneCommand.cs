namespace Scenes.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Repositories;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;

    public class PutSceneCommand
    {
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

        public PutSceneCommand(
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneToSceneMapper,
            IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper)
        {
            this.sceneRepository = sceneRepository;
            this.sceneToSceneMapper = sceneToSceneMapper;
            this.saveSceneToSceneMapper = saveSceneToSceneMapper;
        }

        public async Task<IActionResult> ExecuteAsync(int sceneId, SaveScene saveScene, CancellationToken cancellationToken)
        {
            var scene = await this.sceneRepository.GetAsync(sceneId, cancellationToken).ConfigureAwait(false);
            if (scene is null)
            {
                return new NotFoundResult();
            }

            this.saveSceneToSceneMapper.Map(saveScene, scene);
            scene = await this.sceneRepository.UpdateAsync(scene, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(scene);

            return new OkObjectResult(sceneViewModel);
        }
    }
}
