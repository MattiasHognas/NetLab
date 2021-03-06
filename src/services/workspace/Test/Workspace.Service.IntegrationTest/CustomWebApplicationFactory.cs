namespace Workspace.Service.IntegrationTest
{
    using System.Net.Http;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Moq;
    using Serilog;
    using Serilog.Events;
    using Workspace.Service.Options;
    using Workspace.Service.Repositories;
    using Workspace.Service.Services;
    using Xunit.Abstractions;

    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        public CustomWebApplicationFactory(ITestOutputHelper testOutputHelper)
        {
            this.ClientOptions.AllowAutoRedirect = false;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.TestOutput(testOutputHelper, LogEventLevel.Verbose)
                .CreateLogger();
        }

        public ApplicationOptions ApplicationOptions { get; private set; } = default!;

        public Mock<IBookRepository> BookRepositoryMock { get; } = new Mock<IBookRepository>(MockBehavior.Strict);

        public Mock<IPageRepository> PageRepositoryMock { get; } = new Mock<IPageRepository>(MockBehavior.Strict);

        public Mock<IClockService> ClockServiceMock { get; } = new Mock<IClockService>(MockBehavior.Strict);

        public Mock<IPrincipalService> PrincipalServiceMock { get; } = new Mock<IPrincipalService>(MockBehavior.Strict);

        public void VerifyAllMocks() => Mock.VerifyAll(this.BookRepositoryMock, this.PageRepositoryMock);

        protected override void ConfigureClient(HttpClient client)
        {
            using (var serviceScope = this.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                this.ApplicationOptions = serviceProvider.GetRequiredService<IOptions<ApplicationOptions>>().Value;
            }

            base.ConfigureClient(client);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder) =>
            builder
                .UseEnvironment("Test")
                .ConfigureServices(this.ConfigureServices);

        protected virtual void ConfigureServices(IServiceCollection services) =>
            services
                .AddSingleton(this.BookRepositoryMock.Object)
                .AddSingleton(this.PageRepositoryMock.Object)
                .AddSingleton(this.ClockServiceMock.Object)
                .AddSingleton(this.PrincipalServiceMock.Object);

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.VerifyAllMocks();
            }

            base.Dispose(disposing);
        }
    }
}
