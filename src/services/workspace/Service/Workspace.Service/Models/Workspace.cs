namespace Workspace.Service.Models
{
    using System;

    /// <summary>
    /// The workspace model.
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// Gets or sets the workspace id.
        /// </summary>
        public int WorkspaceId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset Modified { get; set; }
    }
}
