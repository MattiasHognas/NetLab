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
    /// Workspace controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
    public class WorkspaceController : ControllerBase
    {
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions(Name = WorkspaceControllerRoute.OptionsWorkspaces)]
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
        /// Returns an Allow HTTP header with the allowed HTTP methods for a workspace with a specified id.
        /// </summary>
        /// <param name="workspaceId">The workspace id.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{workspaceId}", Name = WorkspaceControllerRoute.OptionsWorkspace)]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
#pragma warning disable IDE0060, CA1801 // Remove unused parameter
        public IActionResult Options(int workspaceId)
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
        /// Deletes the workspace with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="workspaceId">The workspace id.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Workspace response if the workspace was deleted or a 404 Not Found if a workspace with the specified
        /// id was not found.</returns>
        [HttpDelete("{workspaceId}", Name = WorkspaceControllerRoute.DeleteWorkspace)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The workspace with the specified id was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A workspace with the specified id was not found.", typeof(ProblemDetails))]
        public Task<IActionResult> DeleteAsync(
            [FromServices] DeleteWorkspaceCommand command,
            int workspaceId,
            CancellationToken cancellationToken) => command.ExecuteAsync(workspaceId, cancellationToken);

        /// <summary>
        /// Gets a list of workspace.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="workspaceOptionFilter">The workspace option filter.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a list of workspace, a 400 Bad Request if the page request
        /// parameters are invalid.
        /// </returns>
        [HttpGet("", Name = WorkspaceControllerRoute.GetWorkspace)]
        [HttpHead("", Name = WorkspaceControllerRoute.HeadWorkspace)]
        [SwaggerResponse(StatusCodes.Status200OK, "A list of workspace.", typeof(List<Workspace>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        public Task<IActionResult> GetAsync(
            [FromServices] GetWorkspaceCommand command,
            [FromQuery] WorkspaceOptionFilter workspaceOptionFilter,
            CancellationToken cancellationToken) => command.ExecuteAsync(workspaceOptionFilter, cancellationToken);

        /// <summary>
        /// Patches the workspace with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="workspaceId">The workspace id.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK if the workspace was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a workspace with the specified id was not found.</returns>
        [HttpPatch("{workspaceId}", Name = WorkspaceControllerRoute.PatchWorkspace)]
        [SwaggerResponse(StatusCodes.Status200OK, "The patched workspace with the specified id.", typeof(Workspace))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The patch document is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A workspace with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PatchAsync(
            [FromServices] PatchWorkspaceCommand command,
            int workspaceId,
            [FromBody] JsonPatchDocument<SaveWorkspace> patch,
            CancellationToken cancellationToken) => command.ExecuteAsync(workspaceId, patch, cancellationToken);

        /// <summary>
        /// Creates a new workspace.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="workspace">The workspace to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created workspace or a 400 Bad Request if the workspace is
        /// invalid.</returns>
        [HttpPost("", Name = WorkspaceControllerRoute.PostWorkspace)]
        [SwaggerResponse(StatusCodes.Status201Created, "The workspace was created.", typeof(Workspace))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The workspace is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PostAsync(
            [FromServices] PostWorkspaceCommand command,
            [FromBody] SaveWorkspace workspace,
            CancellationToken cancellationToken) => command.ExecuteAsync(workspace, cancellationToken);

        /// <summary>
        /// Updates an existing workspace with the specified id.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="workspaceId">The workspace identifier.</param>
        /// <param name="workspace">The workspace to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated workspace, a 400 Bad Request if the workspace is invalid or a
        /// or a 404 Not Found if a workspace with the specified id was not found.</returns>
        [HttpPut("{workspaceId}", Name = WorkspaceControllerRoute.PutWorkspace)]
        [SwaggerResponse(StatusCodes.Status200OK, "The workspace was updated.", typeof(Workspace))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The workspace is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A workspace with the specified id could not be found.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public Task<IActionResult> PutAsync(
            [FromServices] PutWorkspaceCommand command,
            int workspaceId,
            [FromBody] SaveWorkspace workspace,
            CancellationToken cancellationToken) => command.ExecuteAsync(workspaceId, workspace, cancellationToken);
    }
}
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
