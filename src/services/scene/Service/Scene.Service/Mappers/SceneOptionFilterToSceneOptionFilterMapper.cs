namespace Scene.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Scene.Service.ViewModels;

    /// <summary>
    /// Mapper for scene option filter viewmodel to scene option filter model.
    /// </summary>
    public class SceneOptionFilterToSceneOptionFilterMapper : IMapper<SceneOptionFilter, Models.SceneOptionFilter>
    {
        /// <summary>
        /// Map scene option filter viewmodel to scene option filter model.
        /// </summary>
        /// <param name="source">The scene option filter viewmodel.</param>
        /// <param name="destination">The scene option filter model.</param>
        public void Map(SceneOptionFilter source, Models.SceneOptionFilter destination)
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
        }
    }
}
