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
        [Range(1, ulong.MaxValue)]
        public long PageId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        [Range(1, ulong.MaxValue)]
        public long BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Required]
        public string Value { get; set; } = default!;
    }
}
