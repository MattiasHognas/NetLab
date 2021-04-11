namespace Content.Service.Controllers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Content.Service.Commands;
    using Content.Service.Constants;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Content controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class ContentController : ControllerBase
    {
        /// <summary>
        /// Deletes the content with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="contentId">The content id.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the content was deleted or a 404 Not Found if a content with the specified
        /// id was not found.</returns>
        [HttpDelete("{contentId}", Name = ContentControllerRoute.DeleteContent)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The content with the specified id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A content with the specified id was not found.", typeof(ProblemDetails))]
        public Task<IActionResult> DeleteAsync(
            [FromServices] DeleteContentCommand command,
            long contentId,
            CancellationToken cancellationToken) => command.ExecuteAsync(contentId, cancellationToken);

        /// <summary>
        /// Gets a list of content.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="contentOptionFilter">The content option filter.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a list of content, a 400 Bad Request if the page request
        /// parameters are invalid.
        /// </returns>
        [HttpGet(Name = ContentControllerRoute.GetContent)]
        [HttpHead(Name = ContentControllerRoute.HeadContent)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of content.", typeof(List<Content>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetAsync(
            [FromServices] GetContentCommand command,
            [FromQuery] ContentOptionFilter contentOptionFilter,
            CancellationToken cancellationToken) => command.ExecuteAsync(contentOptionFilter, cancellationToken);

        /// <summary>
        /// Creates a new content.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="content">The content to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created content or a 400 Bad Request if the content is
        /// invalid.</returns>
        [HttpPost(Name = ContentControllerRoute.PostContent)]
        [SwaggerResponse(StatusCodes.Status201Created, "The content was created.", typeof(Content))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The content is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PostAsync(
            [FromServices] PostContentCommand command,
            [FromBody] SaveContent content,
            CancellationToken cancellationToken) => command.ExecuteAsync(content, cancellationToken);

        /// <summary>
        /// Updates an existing content with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="contentId">The content identifier.</param>
        /// <param name="content">The content to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated content, a 400 Bad Request if the content is invalid or a
        /// or a 404 Not Found if a content with the specified id was not found.</returns>
        [HttpPut("{contentId}", Name = ContentControllerRoute.PutContent)]
        [SwaggerResponse(StatusCodes.Status200OK, "The content was updated.", typeof(Content))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The content is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A content with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PutAsync(
            [FromServices] PutContentCommand command,
            long contentId,
            [FromBody] SaveContent content,
            CancellationToken cancellationToken) => command.ExecuteAsync(contentId, content, cancellationToken);
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
