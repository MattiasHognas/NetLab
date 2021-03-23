namespace Scenes.Service.Mappers
{
    using System;
    using Scenes.Service.Constants;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;

    public class SceneToSceneMapper : IMapper<Models.Scene, Scene>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public SceneToSceneMapper(
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.linkGenerator = linkGenerator;
        }

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
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Url = new Uri(this.linkGenerator.GetUriByRouteValues(
                this.httpContextAccessor.HttpContext!,
                ScenesControllerRoute.GetScene,
                new { source.SceneId })!);
        }
    }
}
