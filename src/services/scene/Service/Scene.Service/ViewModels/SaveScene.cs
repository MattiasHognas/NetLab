namespace Scene.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A name and description of scene.
    /// </summary>
    public class SaveScene
    {
        /// <summary>
        /// Gets or sets the scene id of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int SceneId { get; set; } = default!;

        /// <summary>
        /// Gets or sets the X1 of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int X1 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the X2 of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int X2 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Y1 of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Y1 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the Y2 of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Y2 { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user id of the scene.
        /// </summary>
        [Range(1, int.MaxValue)]
        public int UserId { get; set; } = default!;
    }
}
