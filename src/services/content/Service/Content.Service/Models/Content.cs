namespace Content.Service.Models
{
    /// <summary>
    /// The content model.
    /// </summary>
    public class Content : BaseEntity
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        public ulong ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        public ulong PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public ulong BookId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; } = default!;
    }
}
