namespace Content.Service.Models
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
        /// Gets or sets the scene id.
        /// </summary>
        public int SceneId { get; set; }

        /// <summary>
        /// Gets or sets the first X position.
        /// </summary>
        public int X1 { get; set; }

        /// <summary>
        /// Gets or sets the second X position.
        /// </summary>
        public int X2 { get; set; }

        /// <summary>
        /// Gets or sets the first Y position.
        /// </summary>
        public int Y1 { get; set; }

        /// <summary>
        /// Gets or sets the second Y position.
        /// </summary>
        public int Y2 { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

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
