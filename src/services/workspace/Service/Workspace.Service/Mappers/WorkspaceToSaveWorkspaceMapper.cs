namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.Services;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for workspace viewmodel to save workspace model.
    /// </summary>
    public class WorkspaceToSaveWorkspaceMapper : IMapper<Models.Workspace, SaveWorkspace>, IMapper<SaveWorkspace, Models.Workspace>
    {
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceToSaveWorkspaceMapper"/> class.
        /// </summary>
        /// <param name="clockService">The clock service.</param>
        public WorkspaceToSaveWorkspaceMapper(IClockService clockService) =>
            this.clockService = clockService;

        /// <summary>
        /// Map workspace model to save workspace viewmodel.
        /// </summary>
        /// <param name="source">The workspace model.</param>
        /// <param name="destination">The save workspace viewmodel.</param>
        public void Map(Models.Workspace source, SaveWorkspace destination)
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
        }

        /// <summary>
        /// Map save workspace viewmodel to workspace model.
        /// </summary>
        /// <param name="source">The save workspace viewmodel.</param>
        /// <param name="destination">The workspace model.</param>
        public void Map(SaveWorkspace source, Models.Workspace destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var now = this.clockService.UtcNow;

            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }

            destination.WorkspaceId = source.WorkspaceId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Modified = now;
        }
    }
}
