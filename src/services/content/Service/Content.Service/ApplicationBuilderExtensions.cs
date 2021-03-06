namespace Content.Service
{
    using System;
    using System.Linq;
    using Boxed.AspNetCore;
    using Content.Service.Constants;
    using Content.Service.Data;
    using Content.Service.Options;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Events;

    /// <summary>
    /// <see cref="IApplicationBuilder"/> extension methods which extend ASP.NET Core application builder.
    /// </summary>
    internal static partial class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Seed and migrates the database.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The application builder middleware.</returns>
        public static IApplicationBuilder UseSeededDatabase(this IApplicationBuilder application)
        {
            using var scope = application.ApplicationServices.CreateScope();
            var contentContext = scope.ServiceProvider.GetRequiredService<ContentContext>();
            try
            {
                contentContext.Database.Migrate();
            }
            catch (Exception exception)
            {
                Log.Information(exception, "Migration failed");
            }

            ContentContextSeed.Seed(contentContext);
            return application;
        }

        /// <summary>
        /// Uses the static files middleware to serve static files. Also adds the Cache-Control and Pragma HTTP
        /// headers. The cache duration is controlled from configuration.
        /// See http://andrewlock.net/adding-cache-control-headers-to-static-files-in-asp-net-core/.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The application builder with the static files middleware configured.</returns>
        public static IApplicationBuilder UseStaticFilesWithCacheControl(this IApplicationBuilder application)
        {
            var cacheProfile = application
                .ApplicationServices
                .GetRequiredService<CacheProfileOptions>()
                .Where(x => string.Equals(x.Key, CacheProfileName.StaticFiles, StringComparison.Ordinal))
                .Select(x => x.Value)
                .SingleOrDefault();
            return application
                .UseStaticFiles(
                    new StaticFileOptions()
                    {
                        OnPrepareResponse = context =>
                        {
                            if (cacheProfile is not null)
                            {
                                context.Context.ApplyCacheProfile(cacheProfile);
                            }
                        },
                    });
        }

        /// <summary>
        /// Uses custom serilog request logging. Adds additional properties to each log.
        /// See https://github.com/serilog/serilog-aspnetcore.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The application builder with the Serilog middleware configured.</returns>
        public static IApplicationBuilder UseCustomSerilogRequestLogging(this IApplicationBuilder application) =>
            application.UseSerilogRequestLogging(
                options =>
                {
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                    {
                        var request = httpContext.Request;
                        var response = httpContext.Response;
                        var endpoint = httpContext.GetEndpoint();

                        diagnosticContext.Set("Host", request.Host);
                        diagnosticContext.Set("Protocol", request.Protocol);
                        diagnosticContext.Set("Scheme", request.Scheme);

                        if (request.QueryString.HasValue)
                        {
                            diagnosticContext.Set("QueryString", request.QueryString.Value);
                        }

                        if (endpoint is not null)
                        {
                            diagnosticContext.Set("EndpointName", endpoint.DisplayName);
                        }

                        diagnosticContext.Set("ContentType", response.ContentType);
                    };
                    options.GetLevel = GetLevel;

                    static LogEventLevel GetLevel(HttpContext httpContext, double elapsedMilliseconds, Exception exception)
                    {
                        if (exception == null && httpContext.Response.StatusCode <= 499)
                        {
                            if (IsHealthCheckEndpoint(httpContext))
                            {
                                return LogEventLevel.Verbose;
                            }

                            return LogEventLevel.Information;
                        }

                        return LogEventLevel.Error;
                    }

                    static bool IsHealthCheckEndpoint(HttpContext httpContext)
                    {
                        var endpoint = httpContext.GetEndpoint();
                        if (endpoint is not null)
                        {
                            return endpoint.DisplayName == "Health checks";
                        }

                        return false;
                    }
                });

        /// <summary>
        /// Uses custom SwaggerUI request routing.
        /// See https://github.com/swagger-api/swagger-ui.
        /// </summary>
        /// <param name="application">The application builder.</param>
        /// <returns>The application builder with the SwaggerUI middleware configured.</returns>
        public static IApplicationBuilder UseCustomSwaggerUI(this IApplicationBuilder application) =>
            application.UseSwaggerUI(
                options =>
                {
                    // Set the Swagger UI browser document title.
                    options.DocumentTitle = AssemblyInformation.Current.Product;
                    // Set the Swagger UI to render at '/'.
                    options.RoutePrefix = string.Empty;

                    options.DisplayOperationId();
                    options.DisplayRequestDuration();

                    var provider = application.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var apiVersionDescription in provider
                        .ApiVersionDescriptions
                        .OrderByDescending(x => x.ApiVersion))
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                            $"Version {apiVersionDescription.ApiVersion}");
                    }
                });
    }
}
