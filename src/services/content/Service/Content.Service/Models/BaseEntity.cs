namespace Content.Service.Models
{
    using System;

    /// <summary>
    /// The base model.
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the created by user id.
        /// </summary>
        public long CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the modified date.
        /// </summary>
        public DateTimeOffset ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the modified by user id.
        /// </summary>
        public long ModifiedBy { get; set; }
    }
}
