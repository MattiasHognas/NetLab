namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for book viewmodel to save book model.
    /// </summary>
    public class BookToSaveBookMapper : IMapper<Models.Book, SaveBook>, IMapper<SaveBook, Models.Book>
    {
        /// <summary>
        /// Map book model to save book viewmodel.
        /// </summary>
        /// <param name="source">The book model.</param>
        /// <param name="destination">The save book viewmodel.</param>
        public void Map(Models.Book source, SaveBook destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Name = source.Name;
            destination.Description = source.Description;
        }

        /// <summary>
        /// Map save book viewmodel to book model.
        /// </summary>
        /// <param name="source">The save book viewmodel.</param>
        /// <param name="destination">The book model.</param>
        public void Map(SaveBook source, Models.Book destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Name = source.Name;
            destination.Description = source.Description;
        }
    }
}
