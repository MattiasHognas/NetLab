namespace Content.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Content.Service.ViewModels;

    /// <summary>
    /// Mapper for content option filter viewmodel to content option filter model.
    /// </summary>
    public class ContentOptionFilterToContentOptionFilterMapper : IMapper<ContentOptionFilter, Models.ContentOptionFilter>
    {
        /// <summary>
        /// Map content option filter viewmodel to content option filter model.
        /// </summary>
        /// <param name="source">The content option filter viewmodel.</param>
        /// <param name="destination">The content option filter model.</param>
        public void Map(ContentOptionFilter source, Models.ContentOptionFilter destination)
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
            destination.ContentId = source.ContentId;
            destination.WorkspaceId = source.WorkspaceId;
        }
    }
}
