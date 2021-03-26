namespace Scene.Service.Commands
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Mvc;
    using Scene.Service.Repositories;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Get scene command.
    /// </summary>
    public class GetSceneCommand
    {
        private readonly ISceneRepository sceneRepository;
        private readonly IMapper<Models.Scene, Scene> sceneMapper;
        private readonly IMapper<SceneOptionFilter, Models.SceneOptionFilter> sceneOptionFilterMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSceneCommand"/> class.
        /// </summary>
        /// <param name="sceneRepository">The scene repository.</param>
        /// <param name="sceneMapper">The scene mapper.</param>
        /// <param name="sceneOptionFilterMapper">The scene option mapper.</param>
        public GetSceneCommand(
            ISceneRepository sceneRepository,
            IMapper<Models.Scene, Scene> sceneMapper,
            IMapper<SceneOptionFilter, Models.SceneOptionFilter> sceneOptionFilterMapper)
        {
            this.sceneRepository = sceneRepository;
            this.sceneMapper = sceneMapper;
            this.sceneOptionFilterMapper = sceneOptionFilterMapper;
        }

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="sceneOptionFilter">The scene option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(SceneOptionFilter sceneOptionFilter, CancellationToken cancellationToken)
        {
            if (sceneOptionFilter is null)
            {
                throw new ArgumentNullException(nameof(sceneOptionFilter));
            }

            var sceneOptionFilterModel = this.sceneOptionFilterMapper.Map(sceneOptionFilter);
            var scenes = await this.sceneRepository.GetAsync(sceneOptionFilterModel, cancellationToken).ConfigureAwait(false);

            if (scenes is null || !scenes.Any())
            {
                return new NotFoundResult();
            }

            var sceneViewModels = this.sceneMapper.MapList(scenes);
            return new OkObjectResult(sceneViewModels);
        }
    }
}
