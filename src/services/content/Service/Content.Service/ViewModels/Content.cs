namespace Content.Service.ViewModels
{
    using System;

    /// <summary>
    /// The content viewmodel.
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
        public int PageId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public int BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; } = default!;
    }
}
