namespace Scene.Service.ViewModels
{
    using System;

    /// <summary>
    /// A name and description of scene.
    /// </summary>
    public class Scene
    {
        /// <summary>
        /// Gets or sets the scene unique identifier.
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Gets or sets the scene id of the scene.
        /// </summary>
        public int SceneId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the X1 of the scene.
        /// </summary>
        public int X1 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the X2 of the scene.
        /// </summary>
        public int X2 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Y1 of the scene.
        /// </summary>
        public int Y1 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Y2 of the scene.
        /// </summary>
        public int Y2 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user id of the scene.
        /// </summary>
        public int UserId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        public Uri Url { get; set; } = default!;
    }
}
