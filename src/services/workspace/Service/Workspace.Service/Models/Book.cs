namespace Workspace.Service.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// The book model.
    /// </summary>
    public class Book : BaseEntity
    {
        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public ulong BookId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets the pages.
        /// </summary>
        public virtual ICollection<Page> Pages { get; private set; } = new HashSet<Page>();
    }
}
