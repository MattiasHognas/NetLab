﻿namespace Content.Service.Models
{
    using System;

    /// <summary>
    /// The content model.
    /// </summary>
    public class Content
    {
        /// <summary>
        /// Gets or sets the content id.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the page id.
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public int BookId { get; set; }

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
