namespace Workspace.Service.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Net.Http.Headers;
    using Swashbuckle.AspNetCore.Annotations;
    using Workspace.Service.Commands;
    using Workspace.Service.Constants;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Page controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class PageController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = PageControllerRoute.OptionsPages)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Post,
                HttpMethods.Head,
                HttpMethods.Options);
            return this.Ok();
        }

        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a page with a specified id.
        /// </summary>
        /// <param name="pageId">The page id.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{pageId}", Name = PageControllerRoute.OptionsPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
#pragma warning disable IDE0060, CA1801 // Remove unused parameter
        public IActionResult Options(int pageId)
#pragma warning restore IDE0060, CA1801 // Remove unused parameter
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Delete,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Put);
            return this.Ok();
        }

        /// <summary>
        /// Deletes the page with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 NoContent response if the page was deleted or a 404 Not Found if a page with the specified
        /// id was not found.</returns>
        [HttpDelete("{pageId}", Name = PageControllerRoute.DeletePage)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The page with the specified id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified id was not found.", typeof(ProblemDetails))]
        public Task<IActionResult> DeleteAsync(
            [FromServices] DeletePageCommand command,
            int pageId,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageId, cancellationToken);

        /// <summary>
        /// Gets a list of page.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageOptionFilter">The page option filter.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a list of page, a 400 Bad Request if the page request
        /// parameters are invalid.
        /// </returns>
        [HttpGet(Name = PageControllerRoute.GetPage)]
        [HttpHead(Name = PageControllerRoute.HeadPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of page.", typeof(List<Page>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetAsync(
            [FromServices] GetPageCommand command,
            [FromQuery] PageOptionFilter pageOptionFilter,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageOptionFilter, cancellationToken);

        /// <summary>
        /// Patches the page with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageId">The page id.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK if the page was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a page with the specified id was not found.</returns>
        [HttpPatch("{pageId}", Name = PageControllerRoute.PatchPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "The patched page with the specified id.", typeof(Page))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The patch document is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PatchAsync(
            [FromServices] PatchPageCommand command,
            int pageId,
            [FromBody] JsonPatchDocument<SavePage> patch,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageId, patch, cancellationToken);

        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="page">The page to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created page or a 400 Bad Request if the page is
        /// invalid.</returns>
        [HttpPost(Name = PageControllerRoute.PostPage)]
        [SwaggerResponse(StatusCodes.Status201Created, "The page was created.", typeof(Page))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PostAsync(
            [FromServices] PostPageCommand command,
            [FromBody] SavePage page,
            CancellationToken cancellationToken) => command.ExecuteAsync(page, cancellationToken);

        /// <summary>
        /// Updates an existing page with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageId">The page identifier.</param>
        /// <param name="page">The page to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated page, a 400 Bad Request if the page is invalid or a
        /// or a 404 Not Found if a page with the specified id was not found.</returns>
        [HttpPut("{pageId}", Name = PageControllerRoute.PutPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "The page was updated.", typeof(Page))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PutAsync(
            [FromServices] PutPageCommand command,
            int pageId,
            [FromBody] SavePage page,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageId, page, cancellationToken);
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
