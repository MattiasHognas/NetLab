namespace Scenes.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Constants;
    using Scenes.Service.Repositories;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;

    public class PostSceneCommand
    {
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

        public PostSceneCommand(
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneToSceneMapper,
            IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper)
        {
            this.sceneRepository = sceneRepository;
            this.sceneToSceneMapper = sceneToSceneMapper;
            this.saveSceneToSceneMapper = saveSceneToSceneMapper;
        }

        public async Task<IActionResult> ExecuteAsync(SaveScene saveScene, CancellationToken cancellationToken)
        {
            var scene = this.saveSceneToSceneMapper.Map(saveScene);
            scene = await this.sceneRepository.AddAsync(scene, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(scene);

            return new CreatedAtRouteResult(
                ScenesControllerRoute.GetScene,
                new { sceneId = sceneViewModel.SceneId },
                sceneViewModel);
        }
    }
}
