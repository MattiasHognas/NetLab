namespace Scenes.Service.Commands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Repositories;
    using Microsoft.AspNetCore.Mvc;

    public class DeleteSceneCommand
    {
        private readonly ISceneRepository sceneRepository;

        public DeleteSceneCommand(ISceneRepository sceneRepository) =>
            this.sceneRepository = sceneRepository;

        public async Task<IActionResult> ExecuteAsync(int sceneId, CancellationToken cancellationToken)
        {
            var scene = await this.sceneRepository.GetAsync(sceneId, cancellationToken).ConfigureAwait(false);
            if (scene is null)
            {
                return new NotFoundResult();
            }

            await this.sceneRepository.DeleteAsync(scene, cancellationToken).ConfigureAwait(false);

            return new NoContentResult();
        }
    }
}
