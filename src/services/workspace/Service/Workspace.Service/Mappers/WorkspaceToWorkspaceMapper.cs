namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Workspace.Service.Constants;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for workspace viewmodel to workspace model.
    /// </summary>
    public class WorkspaceToWorkspaceMapper : IMapper<Models.Workspace, Workspace>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceToWorkspaceMapper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="linkGenerator">The link generator.</param>
        public WorkspaceToWorkspaceMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Map workspace viewmodel to workspace model.
        /// </summary>
        /// <param name="source">The workspace model.</param>
        /// <param name="destination">The workspace viewmodel.</param>
        public void Map(Models.Workspace source, Workspace destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.WorkspaceId = source.WorkspaceId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                WorkspaceControllerRoute.GetWorkspace,
                new { source.WorkspaceId })!);
        }
    }
}
