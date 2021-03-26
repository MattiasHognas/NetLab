namespace Workspace.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A name and description of workspace.
    /// </summary>
    public class SaveWorkspace
    {
        /// <summary>
        /// Gets or sets the workspace id of the workspace.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int WorkspaceId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the workspace.
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the workspace.
        /// </summary>
        [Required]
        public string Description { get; set; } = default!;
    }
}
