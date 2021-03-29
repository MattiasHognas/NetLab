namespace Content.Service.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// The content model.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        [Key]
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
