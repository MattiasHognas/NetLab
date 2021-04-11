namespace Content.Service.Models
{
    /// <summary>
    /// The content option filter model.
    /// </summary>
    public class ContentOptionFilter
    {
        /// <summary>
        /// Gets or sets the content id which is filtered.
        /// </summary>
        public long? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page id which is filtered.
        /// </summary>
        public long? PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id which is filtered.
        /// </summary>
        public long? BookId { get; set; }
    }
}
