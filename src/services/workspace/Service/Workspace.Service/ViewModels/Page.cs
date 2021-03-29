namespace Workspace.Service.ViewModels
{
    using System;

    /// <summary>
    /// The page viewmodel.
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets or sets the page id of the page.
        /// </summary>
        public ulong PageId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the book id of the page.
        /// </summary>
        public ulong BookId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the name of the page.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the page.
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; } = default!;
    }
}
