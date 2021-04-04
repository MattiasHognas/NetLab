namespace Workspace.Service
{
    using Boxed.AspNetCore;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Workspace.Service.Commands;
    using Workspace.Service.Data;
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
                .AddSingleton<DeleteBookCommand>()
                .AddSingleton<GetBookCommand>()
                .AddSingleton<PostBookCommand>()
                .AddSingleton<PutBookCommand>()
                .AddSingleton<DeletePageCommand>()
                .AddSingleton<GetPageCommand>()
                .AddSingleton<PostPageCommand>()
                .AddSingleton<PutPageCommand>();

        /// <summary>
        /// Adds mapping middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with mapping services added.</returns>
        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services
                .AddSingleton<IMapper<Models.Book, Book>, BookToBookMapper>()
                .AddSingleton<IMapper<Models.Book, SaveBook>, BookToSaveBookMapper>()
                .AddSingleton<IMapper<SaveBook, Models.Book>, BookToSaveBookMapper>()
                .AddSingleton<IMapper<BookOptionFilter, Models.BookOptionFilter>, BookOptionFilterToBookOptionFilterMapper>()
                .AddSingleton<IMapper<Models.Page, Page>, PageToPageMapper>()
                .AddSingleton<IMapper<Models.Page, SavePage>, PageToSavePageMapper>()
                .AddSingleton<IMapper<SavePage, Models.Page>, PageToSavePageMapper>()
                .AddSingleton<IMapper<PageOptionFilter, Models.PageOptionFilter>, PageOptionFilterToPageOptionFilterMapper>();

        /// <summary>
        /// Adds repository middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with repository services added.</returns>
        public static IServiceCollection AddProjectRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<IBookRepository, BookRepository>()
                .AddSingleton<IPageRepository, PageRepository>();

        /// <summary>
        /// Adds service middlewares to the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>The services with service services added.</returns>
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
        /// <param name="webHostEnvironment">The environment the application is running under. This can be Development,
        /// Staging or Production by default. See http://docs.asp.net/en/latest/fundamentals/environments.html.</param>
        /// <returns>The services with contexts added.</returns>
        public static IServiceCollection AddProjectContexts(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment webHostEnvironment) =>
            services
                .AddIfElse(
                    webHostEnvironment.IsDevelopment() || webHostEnvironment.IsEnvironment("Test"),
                    services => services.AddDbContextFactory<WorkspaceContext>(
                        options =>
                        {
                            options.UseInMemoryDatabase("Workspace");
                            options.UseLazyLoadingProxies(true);
                        },
                        ServiceLifetime.Singleton),
                    services => services.AddDbContextFactory<WorkspaceContext>(
                        options =>
                        {
                            options.UseSqlServer(configuration.GetConnectionString("ServiceConnection"));
                            options.UseLazyLoadingProxies(true);
                        },
                        ServiceLifetime.Singleton))
                .AddTransient(serviceProvider => serviceProvider.GetRequiredService<IDbContextFactory<WorkspaceContext>>().CreateDbContext());
    }
}
