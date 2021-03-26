namespace Content.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Content.Service.Services;
    using Content.Service.ViewModels;

    /// <summary>
    /// Mapper for content viewmodel to save content model.
    /// </summary>
    public class ContentToSaveContentMapper : IMapper<Models.Content, SaveContent>, IMapper<SaveContent, Models.Content>
    {
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentToSaveContentMapper"/> class.
        /// </summary>
        /// <param name="clockService">The clock service.</param>
        public ContentToSaveContentMapper(IClockService clockService) =>
            this.clockService = clockService;

        /// <summary>
        /// Map content model to save content viewmodel.
        /// </summary>
        /// <param name="source">The content model.</param>
        /// <param name="destination">The save content viewmodel.</param>
        public void Map(Models.Content source, SaveContent destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.PageId = source.PageId;
            destination.WorkspaceId = source.WorkspaceId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
        }

        /// <summary>
        /// Map save content viewmodel to content model.
        /// </summary>
        /// <param name="source">The save content viewmodel.</param>
        /// <param name="destination">The content model.</param>
        public void Map(SaveContent source, Models.Content destination)
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

            destination.PageId = source.PageId;
            destination.WorkspaceId = source.WorkspaceId;
            destination.X1 = source.X1;
            destination.X2 = source.X2;
            destination.Y1 = source.Y1;
            destination.Y2 = source.Y2;
            destination.Modified = now;
        }
    }
}
