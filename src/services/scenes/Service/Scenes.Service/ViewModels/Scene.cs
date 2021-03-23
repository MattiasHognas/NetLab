namespace Scenes.Service.ViewModels
{
    using System;

    /// <summary>
    /// A name and description of scene.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Gets or sets the scenes unique identifier.
        /// </summary>
        /// <example>1</example>
        public int SceneId { get; set; }

        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        /// <example>My scene</example>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the scene.
        /// </summary>
        /// <example>Description for my scene</example>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        /// <example>/scenes/1</example>
        public Uri Url { get; set; } = default!;
    }
}
