namespace Content.Service.Models
{
    /// <summary>
    /// The content option filter.
    /// </summary>
    public class ContentOptionFilter
    {
        /// <summary>
        /// Gets or sets the content id which is filtered.
        /// </summary>
        public int? ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page id which is filtered.
        /// </summary>
        public int? PageId { get; set; }

        /// <summary>
        /// Gets or sets the scene id which is filtered.
        /// </summary>
        public int? WorkspaceId { get; set; }
    }
}
