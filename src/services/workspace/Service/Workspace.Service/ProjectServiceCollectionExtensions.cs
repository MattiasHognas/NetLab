namespace Workspace.Service
{
    using Boxed.Mapping;
    using Microsoft.Extensions.DependencyInjection;
    using Workspace.Service.Commands;
    using Workspace.Service.Mappers;
    using Workspace.Service.Repositories;
    using Workspace.Service.Services;
    using Workspace.Service.ViewModels;

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
                .AddSingleton<DeleteWorkspaceCommand>()
                .AddSingleton<GetWorkspaceCommand>()
                .AddSingleton<PatchWorkspaceCommand>()
                .AddSingleton<PostWorkspaceCommand>()
                .AddSingleton<PutWorkspaceCommand>();

        /// <summary>
        /// Adds mapping middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with mapping services added.</returns>
        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Workspace, Workspace>, WorkspaceToWorkspaceMapper>()
                .AddSingleton<IMapper<Models.Workspace, SaveWorkspace>, WorkspaceToSaveWorkspaceMapper>()
                .AddSingleton<IMapper<SaveWorkspace, Models.Workspace>, WorkspaceToSaveWorkspaceMapper>()
                .AddSingleton<IMapper<WorkspaceOptionFilter, Models.WorkspaceOptionFilter>, WorkspaceOptionFilterToWorkspaceOptionFilterMapper>();

        /// <summary>
        /// Adds repository middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with repository services added.</returns>
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<IWorkspaceRepository, WorkspaceRepository>();

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
