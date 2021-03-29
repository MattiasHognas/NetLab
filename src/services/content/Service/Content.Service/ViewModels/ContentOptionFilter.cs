namespace Content.Service.ViewModels
{
    /// <summary>
    /// The content option filter viewmodel.
    /// </summary>
    public class ContentOptionFilter
    {
        /// <summary>
        /// Gets or sets the content id which is filtered.
        /// </summary>
        public ulong? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the scene id which is filtered.
        /// </summary>
        public ulong? PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id which is filtered.
        /// </summary>
        public ulong? BookId { get; set; }
    }
}
