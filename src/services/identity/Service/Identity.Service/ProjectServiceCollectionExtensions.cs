namespace Identity.Service
{
    // using Identity.Service.Commands;
    using Identity.Service.Data;
    // using Identity.Service.Mappers;
    using Identity.Service.Repositories;
    using Identity.Service.Services;
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
        /// <returns>The services with command services added.</returns>
        public static IServiceCollection AddProjectCommands(this IServiceCollection services) =>
            services;

        /// <summary>
        /// Adds mapping middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with mapping services added.</returns>
        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services;

        /// <summary>
        /// Adds repository middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with repositories added.</returns>
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddScoped<IAuthRepository, AuthRepository>();

        /// <summary>
        /// Adds service middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with services added.</returns>
        public static IServiceCollection AddProjectServices(this IServiceCollection services) =>
            services
                .AddSingleton<IClockService, ClockService>()
                .AddSingleton<IPrincipalService, PrincipalService>()
                .AddScoped<IUserService, UserService>();

        /// <summary>
        /// Adds context middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The application configuration, where key value pair settings are stored. See
        /// http://docs.asp.net/en/latest/fundamentals/configuration.html.</param>
        /// <returns>The services with context added.</returns>
        public static IServiceCollection AddProjectContexts(this IServiceCollection services, IConfiguration configuration) =>
            services
                .AddDbContextFactory<AppDbContext>(
                    options =>
                    {
                        var serviceConnection = configuration.GetConnectionString("ServiceConnection");
                        Log.Information("Connectionstring: " + serviceConnection);
                        options.UseNpgsql(serviceConnection);
                        options.UseLazyLoadingProxies();
                    },
                    ServiceLifetime.Singleton)
                .AddTransient(serviceProvider => serviceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext());
    }
}
