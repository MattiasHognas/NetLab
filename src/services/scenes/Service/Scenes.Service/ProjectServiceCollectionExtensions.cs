namespace Scenes.Service
{
    using Scenes.Service.Commands;
    using Scenes.Service.Mappers;
    using Scenes.Service.Repositories;
    using Scenes.Service.Services;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// <see cref="IServiceCollection"/> extension methods add project services.
    /// </summary>
    /// <remarks>
    /// AddSingleton - Only one instance is ever created and returned.
    /// AddScoped - A new instance is created and returned for each request/response cycle.
    /// AddTransient - A new instance is created and returned each time.
    /// </remarks>
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddSingleton<DeleteSceneCommand>()
                .AddSingleton<GetSceneCommand>()
                .AddSingleton<GetScenePageCommand>()
                .AddSingleton<PatchSceneCommand>()
                .AddSingleton<PostSceneCommand>()
                .AddSingleton<PutSceneCommand>();

        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Scene, Scene>, SceneToSceneMapper>()
                .AddSingleton<IMapper<Models.Scene, SaveScene>, SceneToSaveSceneMapper>()
                .AddSingleton<IMapper<SaveScene, Models.Scene>, SceneToSaveSceneMapper>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<ISceneRepository, SceneRepository>();

        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>();
    }
}
