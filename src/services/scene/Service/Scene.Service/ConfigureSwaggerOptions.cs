namespace Scene.Service
{
    using Boxed.AspNetCore.Swagger;
    using Boxed.AspNetCore.Swagger.OperationFilters;
    using Boxed.AspNetCore.Swagger.SchemaFilters;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using Scene.Service.OperationFilters;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Configure Swagger.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The api version description provider.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
            this.provider = provider;

        /// <summary>
        /// Set swagger options.
        /// </summary>
        /// <param name="options">The options.</param>
        public void Configure(SwaggerGenOptions options)
        {
            options.DescribeAllParametersInCamelCase();
            options.EnableAnnotations();

            // Add the XML comment file for this assembly, so its scenes can be displayed.
            options.IncludeXmlCommentsIfExists(typeof(Startup).Assembly);

            options.OperationFilter<ApiVersionOperationFilter>();
            options.OperationFilter<ClaimsOperationFilter>();
            options.OperationFilter<ForbiddenResponseOperationFilter>();
            options.OperationFilter<UnauthorizedResponseOperationFilter>();

            // Show a default and example model for JsonPatchDocument<T>.
            options.SchemaFilter<JsonPatchDocumentSchemaFilter>();

            foreach (var apiVersionDescription in this.provider.ApiVersionDescriptions)
            {
                var info = new OpenApiInfo()
                {
                    Title = AssemblyInformation.Current.Product,
                    Description = apiVersionDescription.IsDeprecated ?
                        $"{AssemblyInformation.Current.Description} This API version has been deprecated." :
                        AssemblyInformation.Current.Description,
                    Version = apiVersionDescription.ApiVersion.ToString(),
                };
                options.SwaggerDoc(apiVersionDescription.GroupName, info);
            }
        }
    }
}
