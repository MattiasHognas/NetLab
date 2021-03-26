namespace Content.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Content.Service.Constants;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    /// <summary>
    /// Mapper for content viewmodel to content model.
    /// </summary>
    public class ContentToContentMapper : IMapper<Models.Content, Content>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentToContentMapper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="linkGenerator">The link generator.</param>
        public ContentToContentMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Map content viewmodel to content model.
        /// </summary>
        /// <param name="source">The content model.</param>
        /// <param name="destination">The content viewmodel.</param>
        public void Map(Models.Content source, Content destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.ContentId = source.ContentId;
            destination.SceneId = source.SceneId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
            destination.UserId = source.UserId;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                ContentControllerRoute.GetContent,
                new { source.ContentId })!);
        }
    }
}
