namespace Scene.Service.Commands
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Scene.Service.Repositories;

    /// <summary>
    /// Delete scene command.
    /// </summary>
    public class DeleteSceneCommand
    {
        private readonly ISceneRepository sceneRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSceneCommand"/> class.
        /// </summary>
        /// <param name="sceneRepository">The scene repository.</param>
        public DeleteSceneCommand(ISceneRepository sceneRepository) =>
            this.sceneRepository = sceneRepository;

        /// <summary>
        /// Execute async.
        /// </summary>
        /// <param name="sceneId">The scene id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An action result.</returns>
        public async Task<IActionResult> ExecuteAsync(int sceneId, CancellationToken cancellationToken)
        {
            var filters = new Models.SceneOptionFilter { ContentId = sceneId };
            var scene = await this.sceneRepository.GetAsync(filters, cancellationToken).ConfigureAwait(false);
            if (scene is null || !scene.Any())
            {
                return new NotFoundResult();
            }

            var item = scene.First();
            await this.sceneRepository.DeleteAsync(item, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
