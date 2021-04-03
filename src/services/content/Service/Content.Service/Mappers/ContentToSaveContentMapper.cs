namespace Content.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Content.Service.ViewModels;

    /// <summary>
    /// Mapper for content viewmodel to save content model.
    /// </summary>
    public class ContentToSaveContentMapper : IMapper<Models.Content, SaveContent>, IMapper<SaveContent, Models.Content>
    {
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
            destination.BookId = source.BookId;
            destination.Value = source.Value;
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

            destination.PageId = source.PageId;
            destination.BookId = source.BookId;
            destination.Value = source.Value;
        }
    }
}
