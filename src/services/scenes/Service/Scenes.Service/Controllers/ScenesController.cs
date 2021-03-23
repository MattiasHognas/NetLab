namespace Scenes.Service.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.Commands;
    using Scenes.Service.Constants;
    using Scenes.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using Swashbuckle.AspNetCore.Annotations;

    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class ScenesController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = ScenesControllerRoute.OptionsScenes)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return this.Ok();
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a scene with the specified unique identifier.
        /// </summary>
        /// <param name="sceneId">The scenes unique identifier.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{sceneId}", Name = ScenesControllerRoute.OptionsScene)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
#pragma warning disable IDE0060, CA1801 // Remove unused parameter
        public IActionResult Options(int sceneId)
#pragma warning restore IDE0060, CA1801 // Remove unused parameter
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Post,
                HttpMethods.Put);
            return this.Ok();
        }

        /// <summary>
        /// Deletes the scene with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sceneId">The scenes unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the scene was deleted or a 404 Not Found if a scene with the specified
        /// unique identifier was not found.</returns>
        [HttpDelete("{sceneId}", Name = ScenesControllerRoute.DeleteScene)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The scene with the specified unique identifier was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A scene with the specified unique identifier was not found.", typeof(ProblemDetails))]
        public Task<IActionResult> DeleteAsync(
            [FromServices] DeleteSceneCommand command,
            int sceneId,
            CancellationToken cancellationToken) => command.ExecuteAsync(sceneId, cancellationToken);

        /// <summary>
        /// Gets the scene with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sceneId">The scenes unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the scene or a 404 Not Found if a scene with the specified unique
        /// identifier was not found.</returns>
        [HttpGet("{sceneId}", Name = ScenesControllerRoute.GetScene)]
        [HttpHead("{sceneId}", Name = ScenesControllerRoute.HeadScene)]
        [SwaggerResponse(StatusCodes.Status200OK, "The scene with the specified unique identifier.", typeof(Scene))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The scene has not changed since the date given in the If-Modified-Since HTTP header.", typeof(void))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A scene with the specified unique identifier could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetAsync(
            [FromServices] GetSceneCommand command,
            int sceneId,
            CancellationToken cancellationToken) => command.ExecuteAsync(sceneId, cancellationToken);

        /// <summary>
        /// Gets a collection of scenes.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageOptions">The page options.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a collection of scenes, a 400 Bad Request if the page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        [HttpGet("", Name = ScenesControllerRoute.GetScenePage)]
        [HttpHead("", Name = ScenesControllerRoute.HeadScenePage)]
        [SwaggerResponse(StatusCodes.Status200OK, "A collection of scenes for the specified page.", typeof(Connection<Scene>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetPageAsync(
            [FromServices] GetScenePageCommand command,
            [FromQuery] PageOptions pageOptions,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageOptions, cancellationToken);

        /// <summary>
        /// Patches the scene with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sceneId">The scenes unique identifier.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK if the scene was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a scene with the specified unique identifier was not found.</returns>
        [HttpPatch("{sceneId}", Name = ScenesControllerRoute.PatchScene)]
        [SwaggerResponse(StatusCodes.Status200OK, "The patched scene with the specified unique identifier.", typeof(Scene))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The patch document is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A scene with the specified unique identifier could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PatchAsync(
            [FromServices] PatchSceneCommand command,
            int sceneId,
            [FromBody] JsonPatchDocument<SaveScene> patch,
            CancellationToken cancellationToken) => command.ExecuteAsync(sceneId, patch, cancellationToken);

        /// <summary>
        /// Creates a new scene.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="scene">The scene to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created scene or a 400 Bad Request if the scene is
        /// invalid.</returns>
        [HttpPost("", Name = ScenesControllerRoute.PostScene)]
        [SwaggerResponse(StatusCodes.Status201Created, "The scene was created.", typeof(Scene))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The scene is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PostAsync(
            [FromServices] PostSceneCommand command,
            [FromBody] SaveScene scene,
            CancellationToken cancellationToken) => command.ExecuteAsync(scene, cancellationToken);

        /// <summary>
        /// Updates an existing scene with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="sceneId">The scene identifier.</param>
        /// <param name="scene">The scene to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated scene, a 400 Bad Request if the scene is invalid or a
        /// or a 404 Not Found if a scene with the specified unique identifier was not found.</returns>
        [HttpPut("{sceneId}", Name = ScenesControllerRoute.PutScene)]
        [SwaggerResponse(StatusCodes.Status200OK, "The scene was updated.", typeof(Scene))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The scene is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A scene with the specified unique identifier could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PutAsync(
            [FromServices] PutSceneCommand command,
            int sceneId,
            [FromBody] SaveScene scene,
            CancellationToken cancellationToken) => command.ExecuteAsync(sceneId, scene, cancellationToken);
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
