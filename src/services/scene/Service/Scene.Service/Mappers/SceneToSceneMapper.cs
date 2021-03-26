namespace Scene.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using Scene.Service.Constants;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Mapper for scene viewmodel to scene model.
    /// </summary>
    public class SceneToSceneMapper : IMapper<Models.Scene, Scene>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneToSceneMapper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="linkGenerator">The link generator.</param>
        public SceneToSceneMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

        /// <summary>
        /// Map scene viewmodel to scene model.
        /// </summary>
        /// <param name="source">The scene model.</param>
        /// <param name="destination">The scene viewmodel.</param>
        public void Map(Models.Scene source, Scene destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.SceneId = source.SceneId;
            destination.SceneId = source.SceneId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
            destination.UserId = source.UserId;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                SceneControllerRoute.GetScene,
                new { source.SceneId })!);
        }
    }
}
