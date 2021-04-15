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
            long bookId,
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
            long bookId,
            [FromBody] SaveBook book,
            CancellationToken cancellationToken) => command.ExecuteAsync(bookId, book, cancellationToken);
    }
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
}
