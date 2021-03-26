namespace Scene.Service.Repositories
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Scene.Service.Models;

    /// <summary>
    /// Scene repository.
    /// </summary>
    public interface ISceneRepository
    {
        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A scene.</returns>
        Task<Scene> AddAsync(Scene scene, CancellationToken cancellationToken);

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        Task DeleteAsync(Scene scene, CancellationToken cancellationToken);

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="sceneOptionFilter">The scene option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of scene.</returns>
        Task<List<Scene>> GetAsync(SceneOptionFilter sceneOptionFilter, CancellationToken cancellationToken);

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A scene.</returns>
        Task<Scene> UpdateAsync(Scene scene, CancellationToken cancellationToken);
    }
}
