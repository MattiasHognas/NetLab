namespace Scenes.Service.Mappers
{
    using System;
    using Scenes.Service.Services;
    using Scenes.Service.ViewModels;
    using Boxed.Mapping;

    public class SceneToSaveSceneMapper : IMapper<Models.Scene, SaveScene>, IMapper<SaveScene, Models.Scene>
    {
        private readonly IClockService clockService;

        public SceneToSaveSceneMapper(IClockService clockService) =>
            this.clockService = clockService;

        public void Map(Models.Scene source, SaveScene destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Name = source.Name;
            destination.Description = source.Description;
        }

        public void Map(SaveScene source, Models.Scene destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            var now = this.clockService.UtcNow;

            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }

            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Modified = now;
        }
    }
}
