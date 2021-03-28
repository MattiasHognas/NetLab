namespace Workspace.Service.Models
{
    using System;

    /// <summary>
    /// The page model.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset Modified { get; set; }
    }
}
