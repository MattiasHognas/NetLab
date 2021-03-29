namespace Content.Service
{
    using Boxed.Mapping;
    using Content.Service.Commands;
    using Content.Service.Data;
    using Content.Service.Mappers;
    using Content.Service.Repositories;
    using Content.Service.Services;
    using Content.Service.ViewModels;
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
        /// <summary>
        /// Adds command middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with commands added.</returns>
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services
                .AddScoped<DeleteContentCommand>()
                .AddScoped<GetContentCommand>()
                .AddScoped<PatchContentCommand>()
                .AddScoped<PostContentCommand>()
                .AddScoped<PutContentCommand>();

        /// <summary>
        /// Adds mapping middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with mappings added.</returns>
        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Content, Content>, ContentToContentMapper>()
                .AddSingleton<IMapper<Models.Content, SaveContent>, ContentToSaveContentMapper>()
                .AddSingleton<IMapper<SaveContent, Models.Content>, ContentToSaveContentMapper>()
                .AddSingleton<IMapper<ContentOptionFilter, Models.ContentOptionFilter>, ContentOptionFilterToContentOptionFilterMapper>();

        /// <summary>
        /// Adds repository middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with repositories added.</returns>
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<IContentRepository, ContentRepository>();

        /// <summary>
        /// Adds service middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with service services added.</returns>
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>();

        /// <summary>
        /// Adds context middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with contexts added.</returns>
        public static IServiceCollection AddProjectContexts(this IServiceCollection services) =>
            services
                .AddScoped<ContentsContext>();
    }
}
