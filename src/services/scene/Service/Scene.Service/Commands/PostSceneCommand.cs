namespace Scene.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Scene.Service.Constants;
    using Scene.Service.Repositories;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Post scene command.
    /// </summary>
    public class PostSceneCommand
    {
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostSceneCommand"/> class.
        /// </summary>
        /// <param name="sceneRepository">The scene repository.</param>
        /// <param name="sceneToSceneMapper">The scene to scene mapper.</param>
        /// <param name="saveSceneToSceneMapper">The save scene to scene mapper.</param>
        public PostSceneCommand(
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneToSceneMapper,
            IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper)
        {
            this.sceneRepository = sceneRepository;
            this.sceneToSceneMapper = sceneToSceneMapper;
            this.saveSceneToSceneMapper = saveSceneToSceneMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="saveScene">The save scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SaveScene saveScene, CancellationToken cancellationToken)
        {
            var scene = this.saveSceneToSceneMapper.Map(saveScene);
            scene = await this.sceneRepository.AddAsync(scene, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(scene);

            var filters = new Models.SceneOptionFilter { SceneId = sceneViewModel.SceneId };
            return new CreatedAtRouteResult(
                SceneControllerRoute.GetScene,
                filters,
                sceneViewModel);
        }
    }
}
