namespace Scenes.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Models;

    public interface ISceneRepository
    {
        Task<Scene> AddAsync(Scene scene, CancellationToken cancellationToken);

        Task DeleteAsync(Scene scene, CancellationToken cancellationToken);

        Task<Scene?> GetAsync(int sceneId, CancellationToken cancellationToken);

        Task<List<Scene>> GetScenesAsync(
            int? first,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken);

        Task<List<Scene>> GetScenesReverseAsync(
            int? last,
            DateTimeOffset? createdAfter,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken);

        Task<bool> GetHasNextPageAsync(
            int? first,
            DateTimeOffset? createdAfter,
            CancellationToken cancellationToken);

        Task<bool> GetHasPreviousPageAsync(
            int? last,
            DateTimeOffset? createdBefore,
            CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken);

        Task<Scene> UpdateAsync(Scene scene, CancellationToken cancellationToken);
    }
}
