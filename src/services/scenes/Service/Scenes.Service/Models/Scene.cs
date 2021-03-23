namespace Scenes.Service.Models
{
    using System;

    public class Scene
    {
        public int SceneId { get; set; }

        public DateTimeOffset Created { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public DateTimeOffset Modified { get; set; }
    }
}
