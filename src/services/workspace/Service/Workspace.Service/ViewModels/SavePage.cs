namespace Workspace.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The page save viewmodel.
    /// </summary>
    public class SavePage
    {
        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        [Range(1, ulong.MaxValue)]
        public ulong BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;
    }
}
