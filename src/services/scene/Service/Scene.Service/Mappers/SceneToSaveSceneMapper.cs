namespace Scene.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Scene.Service.Services;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Mapper for scene viewmodel to save scene model.
    /// </summary>
    public class SceneToSaveSceneMapper : IMapper<Models.Scene, SaveScene>, IMapper<SaveScene, Models.Scene>
    {
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneToSaveSceneMapper"/> class.
        /// </summary>
        /// <param name="clockService">The clock service.</param>
        public SceneToSaveSceneMapper(IClockService clockService) =>
            this.clockService = clockService;

        /// <summary>
        /// Map scene model to save scene viewmodel.
        /// </summary>
        /// <param name="source">The scene model.</param>
        /// <param name="destination">The save scene viewmodel.</param>
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

            destination.SceneId = source.SceneId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
            destination.UserId = source.UserId;
        }

        /// <summary>
        /// Map save scene viewmodel to scene model.
        /// </summary>
        /// <param name="source">The save scene viewmodel.</param>
        /// <param name="destination">The scene model.</param>
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

            destination.SceneId = source.SceneId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
            destination.UserId = source.UserId;
            destination.Modified = now;
        }
    }
}
