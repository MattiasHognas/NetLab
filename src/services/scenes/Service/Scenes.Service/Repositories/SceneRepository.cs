namespace Scenes.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Models;

    public class SceneRepository : ISceneRepository
    {
        private static readonly List<Scene> Scenes = new()
        {
            new Scene()
            {
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "Lamborghini",
                Description = "Countach",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Scene()
            {
                SceneId = 2,
                Created = DateTimeOffset.UtcNow.AddDays(-5),
                Name = "Mazda",
                Description = "Furai",
                Modified = DateTimeOffset.UtcNow.AddDays(-6),
            },
            new Scene()
            {
                SceneId = 3,
                Created = DateTimeOffset.UtcNow.AddDays(-10),
                Name = "Honda",
                Description = "NSX",
                Modified = DateTimeOffset.UtcNow.AddDays(-3),
            },
            new Scene()
            {
                SceneId = 4,
                Created = DateTimeOffset.UtcNow.AddDays(-3),
                Name = "Lotus",
                Description = "Esprit",
                Modified = DateTimeOffset.UtcNow.AddDays(-3),
            },
            new Scene()
            {
                SceneId = 5,
                Created = DateTimeOffset.UtcNow.AddDays(-12),
                Name = "Mitsubishi",
                Description = "Evo",
                Modified = DateTimeOffset.UtcNow.AddDays(-2),
            },
            new Scene()
            {
                SceneId = 6,
                Created = DateTimeOffset.UtcNow.AddDays(-1),
                Name = "McLaren",
                Description = "F1",
                Modified = DateTimeOffset.UtcNow.AddDays(-1),
            },
        };

        public Task<Scene> AddAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (scene is null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            Scenes.Add(scene);
            scene.SceneId = Scenes.Max(x => x.SceneId) + 1;
            return Task.FromResult(scene);
        }

        public Task DeleteAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (Scenes.Contains(scene))
            {
                Scenes.Remove(scene);
            }

            return Task.CompletedTask;
        }

        public Task<Scene> GetAsync(int sceneId, CancellationToken cancellationToken)
        {
            var scene = Scenes.FirstOrDefault(x => x.SceneId == sceneId);
            return Task.FromResult(scene);
        }

        public Task<List<Scene>> GetScenesAsync(
            int? first,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Scenes
                .OrderBy(x => x.Created)
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter!.Value))
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore!.Value))
                .If(first.HasValue, x => x.Take(first!.Value))
                .ToList());

        public Task<List<Scene>> GetScenesReverseAsync(
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Scenes
                .OrderBy(x => x.Created)
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter!.Value))
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore!.Value))
                .If(last.HasValue, x => x.TakeLast(last!.Value))
                .ToList());

        public Task<bool> GetHasNextPageAsync(
            int? first,
            DateTimeOffset? createdAfter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Scenes
                .OrderBy(x => x.Created)
                .If(createdAfter.HasValue, x => x.Where(y => y.Created > createdAfter!.Value))
                .If(first.HasValue, x => x.Skip(first!.Value))
                .Any());

        public Task<bool> GetHasPreviousPageAsync(
            int? last,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken) =>
            Task.FromResult(Scenes
                .OrderBy(x => x.Created)
                .If(createdBefore.HasValue, x => x.Where(y => y.Created < createdBefore!.Value))
                .If(last.HasValue, x => x.SkipLast(last!.Value))
                .Any());

        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Scenes.Count);

        public Task<Scene> UpdateAsync(Scene scene, CancellationToken cancellationToken)
        {
            if (scene is null)
            {
                throw new ArgumentNullException(nameof(scene));
            }

            var existingScene = Scenes.First(x => x.SceneId == scene.SceneId);
            existingScene.Name = scene.Name;
            existingScene.Description = scene.Description;
            return Task.FromResult(scene);
        }
    }
}
