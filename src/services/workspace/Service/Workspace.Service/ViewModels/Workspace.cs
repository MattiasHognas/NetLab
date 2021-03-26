namespace Workspace.Service.ViewModels
{
    using System;

    /// <summary>
    /// A name and description of workspace.
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// Gets or sets the workspace id of the workspace.
        /// </summary>
        public int WorkspaceId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the workspace.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the workspace.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; } = default!;
    }
}
