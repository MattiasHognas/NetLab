namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Workspace.Service.Constants;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for book viewmodel to book model.
    /// </summary>
    public class BookToBookMapper : IMapper<Models.Book, Book>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookToBookMapper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="linkGenerator">The link generator.</param>
        public BookToBookMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Map book viewmodel to book model.
        /// </summary>
        /// <param name="source">The book model.</param>
        /// <param name="destination">The book viewmodel.</param>
        public void Map(Models.Book source, Book destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.BookId = source.BookId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                BookControllerRoute.GetBook,
                new { source.BookId })!);
        }
    }
}
