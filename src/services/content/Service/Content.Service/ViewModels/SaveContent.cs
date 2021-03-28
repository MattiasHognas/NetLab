namespace Content.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The save content viewmodel.
    /// </summary>
    public class SaveContent
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Required]
        public string Value { get; set; } = default!;
    }
}
