namespace Workspace.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The book save viewmodel.
    /// </summary>
    public class SaveBook
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;
    }
}
