namespace Scene.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Scene.Service.Repositories;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Put scene command.
    /// </summary>
    public class PutSceneCommand
    {
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneToSceneMapper;
        private readonly IMapper<SaveScene, Models.Scene> saveSceneToSceneMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PutSceneCommand"/> class.
        /// </summary>
        /// <param name="sceneRepository">The scene repository.</param>
        /// <param name="sceneToSceneMapper">The scene to scene mapper.</param>
        /// <param name="saveSceneToSceneMapper">The save scene to scene mapper.</param>
        public PutSceneCommand(
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
        /// <param name="sceneId">The scene id.</param>
        /// <param name="saveScene">The save scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int sceneId, SaveScene saveScene, CancellationToken cancellationToken)
        {
            var filters = new Models.SceneOptionFilter { SceneId = sceneId };
            var scene = await this.sceneRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (scene is null || !scene.Any())
            {
                return new NotFoundResult();
            }

            var item = scene.First();
            this.saveSceneToSceneMapper.Map(saveScene, item);
            item = await this.sceneRepository.UpdateAsync(item, cancellationToken).ConfigureAwait(false);
            var sceneViewModel = this.sceneToSceneMapper.Map(item);

            return new OkObjectResult(sceneViewModel);
        }
    }
}
