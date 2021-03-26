namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for workspace option filter viewmodel to workspace option filter model.
    /// </summary>
    public class WorkspaceOptionFilterToWorkspaceOptionFilterMapper : IMapper<WorkspaceOptionFilter, Models.WorkspaceOptionFilter>
    {
        /// <summary>
        /// Map workspace option filter viewmodel to workspace option filter model.
        /// </summary>
        /// <param name="source">The workspace option filter viewmodel.</param>
        /// <param name="destination">The workspace option filter model.</param>
        public void Map(WorkspaceOptionFilter source, Models.WorkspaceOptionFilter destination)
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
        }
    }
}
