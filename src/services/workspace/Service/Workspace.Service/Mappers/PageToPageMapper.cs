namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Workspace.Service.Constants;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for page viewmodel to page model.
    /// </summary>
    public class PageToPageMapper : IMapper<Models.Page, Page>
    {
        private readonly IMapper<Models.Book, Book> bookToBookMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageToPageMapper"/> class.
        /// </summary>
        /// <param name="bookToBookMapper">The book to book mapper.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="linkGenerator">The link generator.</param>
        public PageToPageMapper(
            IMapper<Models.Book, Book> bookToBookMapper,
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.bookToBookMapper = bookToBookMapper;
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Map page viewmodel to page model.
        /// </summary>
        /// <param name="source">The page model.</param>
        /// <param name="destination">The page viewmodel.</param>
        public void Map(Models.Page source, Page destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.PageId = source.PageId;
            destination.BookId = source.Book.BookId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                PageControllerRoute.GetPage,
                new { source.PageId })!);
        }
    }
}
