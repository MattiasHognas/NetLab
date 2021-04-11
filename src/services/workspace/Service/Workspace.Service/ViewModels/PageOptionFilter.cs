namespace Workspace.Service.ViewModels
{
    /// <summary>
    /// The page option filter viewmodel.
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
