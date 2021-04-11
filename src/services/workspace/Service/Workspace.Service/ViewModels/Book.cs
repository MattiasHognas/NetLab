namespace Workspace.Service.ViewModels
{
    using System;

    /// <summary>
    /// The book viewmodel.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Gets or sets the book id.
        /// </summary>
        public long BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; } = default!;
    }
}
