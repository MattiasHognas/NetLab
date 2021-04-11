namespace Content.Service
{
    using Boxed.Mapping;
    using Content.Service.Commands;
    using Content.Service.Data;
    using Content.Service.Mappers;
    using Content.Service.Repositories;
    using Content.Service.Services;
    using Content.Service.ViewModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;

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
                .AddSingleton<DeleteContentCommand>()
                .AddSingleton<GetContentCommand>()
                .AddSingleton<PostContentCommand>()
                .AddSingleton<PutContentCommand>();

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
                .AddSingleton<IContentRepository, ContentRepository>();

        /// <summary>
        /// Adds service middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with services added.</returns>
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<IPrincipalService, PrincipalService>();

        /// <summary>
        /// Adds context middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html.</param>
        /// <returns>The services with context added.</returns>
        public static IServiceCollection AddProjectContexts(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContextFactory<ContentContext>(
                    options =>
                    {
                        var serviceConnection = configuration.GetConnectionString("ServiceConnection");
                        Log.Information("Connectionstring: " + serviceConnection);
                        options.UseNpgsql(serviceConnection);
                        options.UseLazyLoadingProxies();
                    },
                    ServiceLifetime.Singleton)
                .AddTransient(serviceProvider => serviceProvider.GetRequiredService<IDbContextFactory<ContentContext>>().CreateDbContext());
    }
}
