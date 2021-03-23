namespace Scenes.Service.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A name and description of scene.
    /// </summary>
    public class SaveScene
    {
        /// <summary>
        /// Gets or sets the name of the scene.
        /// </summary>
        /// <example>My scene</example>
        [Required]
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the description of the scene.
        /// </summary>
        /// <example>Description for my scene</example>
        [Required]
        public string Description { get; set; } = default!;
    }
}
