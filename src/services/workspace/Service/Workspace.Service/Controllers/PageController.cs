namespace Workspace.Service.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
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
            long pageId,
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
            long pageId,
            [FromBody] SavePage page,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageId, page, cancellationToken);
    }
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
}
