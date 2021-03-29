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
    /// Book controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class BookController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = BookControllerRoute.OptionsBooks)]
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
        /// Returns an Allow HTTP header with the allowed HTTP methods for a book with a specified id.
        /// </summary>
        /// <param name="bookId">The book id.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{bookId}", Name = BookControllerRoute.OptionsBook)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
#pragma warning disable IDE0060, CA1801 // Remove unused parameter
        public IActionResult Options(int bookId)
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
        /// Deletes the book with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="bookId">The book id.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 NoContent response if the book was deleted or a 404 Not Found if a book with the specified
        /// id was not found.</returns>
        [HttpDelete("{bookId}", Name = BookControllerRoute.DeleteBook)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The book with the specified id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A book with the specified id was not found.", typeof(ProblemDetails))]
        public Task<IActionResult> DeleteAsync(
            [FromServices] DeleteBookCommand command,
            ulong bookId,
            CancellationToken cancellationToken) => command.ExecuteAsync(bookId, cancellationToken);

        /// <summary>
        /// Gets a list of book.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="bookOptionFilter">The book option filter.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a list of book, a 400 Bad Request if the page request
        /// parameters are invalid.
        /// </returns>
        [HttpGet(Name = BookControllerRoute.GetBook)]
        [HttpHead(Name = BookControllerRoute.HeadBook)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of book.", typeof(List<Book>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetAsync(
            [FromServices] GetBookCommand command,
            [FromQuery] BookOptionFilter bookOptionFilter,
            CancellationToken cancellationToken) => command.ExecuteAsync(bookOptionFilter, cancellationToken);

        /// <summary>
        /// Patches the book with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="bookId">The book id.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK if the book was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a book with the specified id was not found.</returns>
        [HttpPatch("{bookId}", Name = BookControllerRoute.PatchBook)]
        [SwaggerResponse(StatusCodes.Status200OK, "The patched book with the specified id.", typeof(Book))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The patch document is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A book with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PatchAsync(
            [FromServices] PatchBookCommand command,
            ulong bookId,
            [FromBody] JsonPatchDocument<SaveBook> patch,
            CancellationToken cancellationToken) => command.ExecuteAsync(bookId, patch, cancellationToken);

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="book">The book to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created book or a 400 Bad Request if the book is
        /// invalid.</returns>
        [HttpPost(Name = BookControllerRoute.PostBook)]
        [SwaggerResponse(StatusCodes.Status201Created, "The book was created.", typeof(Book))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The book is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PostAsync(
            [FromServices] PostBookCommand command,
            [FromBody] SaveBook book,
            CancellationToken cancellationToken) => command.ExecuteAsync(book, cancellationToken);

        /// <summary>
        /// Updates an existing book with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="bookId">The book identifier.</param>
        /// <param name="book">The book to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated book, a 400 Bad Request if the book is invalid or a
        /// or a 404 Not Found if a book with the specified id was not found.</returns>
        [HttpPut("{bookId}", Name = BookControllerRoute.PutBook)]
        [SwaggerResponse(StatusCodes.Status200OK, "The book was updated.", typeof(Book))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The book is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A book with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PutAsync(
            [FromServices] PutBookCommand command,
            ulong bookId,
            [FromBody] SaveBook book,
            CancellationToken cancellationToken) => command.ExecuteAsync(bookId, book, cancellationToken);
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
