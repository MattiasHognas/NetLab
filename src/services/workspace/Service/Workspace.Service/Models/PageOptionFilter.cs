namespace Workspace.Service.Models
{
    /// <summary>
    /// The page option filter model.
    /// </summary>
    public class PageOptionFilter
    {
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
