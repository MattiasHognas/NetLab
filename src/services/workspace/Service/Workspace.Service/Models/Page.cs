namespace Workspace.Service.Models
{
    /// <summary>
    /// The page model.
    /// </summary>
    public class Page : BaseEntity
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        public long PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public long BookId { get; set; }

        /// <summary>
        /// Gets or sets the book.
        /// </summary>
        public virtual Book Book { get; set; } = default!;

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
