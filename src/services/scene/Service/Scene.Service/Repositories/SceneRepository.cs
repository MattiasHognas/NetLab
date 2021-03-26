namespace Scene.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Scene.Service.Models;

    /// <summary>
    /// Scene repository.
    /// </summary>
    public class SceneRepository : ISceneRepository
    {
        private static readonly List<Scene> Scene = new()
        {
            new Scene()
            {
                ContentId = 1,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 1,
                X2 = 2,
                Y1 = 1,
                Y2 = 2,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Scene()
            {
                ContentId = 2,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 2,
                X2 = 3,
                Y1 = 2,
                Y2 = 3,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Scene()
            {
                ContentId = 3,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 3,
                X2 = 4,
                Y1 = 3,
                Y2 = 4,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
        };

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A scene.</returns>
        public Task<Scene> AddAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (scene is null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            Scene.Add(scene);
            scene.SceneId = Scene.Max(x => x.SceneId) + 1;
            return Task.FromResult(scene);
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (Scene.Contains(scene))
            {
                Scene.Remove(scene);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="sceneOptionFilter">The scene option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of scene.</returns>
        public Task<List<Scene>> GetAsync(
            SceneOptionFilter sceneOptionFilter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Scene
                .OrderBy(x => x.Created)
                .If(sceneOptionFilter.ContentId.HasValue, x => x.Where(y => y.ContentId == sceneOptionFilter.SceneId))
                .If(sceneOptionFilter.SceneId.HasValue, x => x.Where(y => y.SceneId == sceneOptionFilter.SceneId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Scene.Count);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A scene.</returns>
        public Task<Scene> UpdateAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (scene is null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            var existingScene = Scene.First(x => x.SceneId == scene.SceneId);
            // existingScene.SceneId = scene.SceneId;
            existingScene.X1 = scene.X1;
            existingScene.X2 = scene.X2;
            existingScene.Y1 = scene.Y1;
            existingScene.Y2 = scene.Y2;
            // existingScene.UserId = scene.UserId;
            return Task.FromResult(scene);
        }
    }
}
