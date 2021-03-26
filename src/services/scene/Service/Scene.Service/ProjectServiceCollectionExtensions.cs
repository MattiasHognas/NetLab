namespace Scene.Service
{
    using Boxed.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Scene.Service.Commands;
    using Scene.Service.Mappers;
    using Scene.Service.Repositories;
    using Scene.Service.Services;
    using Scene.Service.ViewModels;

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
        /// <summary>
        /// Adds command middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with command services added.</returns>
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddSingleton<DeleteSceneCommand>()
                .AddSingleton<GetSceneCommand>()
                .AddSingleton<PatchSceneCommand>()
                .AddSingleton<PostSceneCommand>()
                .AddSingleton<PutSceneCommand>();

        /// <summary>
        /// Adds mapping middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with mapping services added.</returns>
        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Scene, Scene>, SceneToSceneMapper>()
                .AddSingleton<IMapper<Models.Scene, SaveScene>, SceneToSaveSceneMapper>()
                .AddSingleton<IMapper<SaveScene, Models.Scene>, SceneToSaveSceneMapper>()
                .AddSingleton<IMapper<SceneOptionFilter, Models.SceneOptionFilter>, SceneOptionFilterToSceneOptionFilterMapper>();

        /// <summary>
        /// Adds repository middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with repository services added.</returns>
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<ISceneRepository, SceneRepository>();

        /// <summary>
        /// Adds service middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with service services added.</returns>
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>();
    }
}
