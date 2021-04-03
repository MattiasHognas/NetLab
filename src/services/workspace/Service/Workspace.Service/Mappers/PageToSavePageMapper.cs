namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for page viewmodel to save page model.
    /// </summary>
    public class PageToSavePageMapper : IMapper<Models.Page, SavePage>, IMapper<SavePage, Models.Page>
    {
        /// <summary>
        /// Map page model to save page viewmodel.
        /// </summary>
        /// <param name="source">The page model.</param>
        /// <param name="destination">The save page viewmodel.</param>
        public void Map(Models.Page source, SavePage destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.BookId = source.Book.BookId;
            destination.Name = source.Name;
            destination.Description = source.Description;
        }

        /// <summary>
        /// Map save page viewmodel to page model.
        /// </summary>
        /// <param name="source">The save page viewmodel.</param>
        /// <param name="destination">The page model.</param>
        public void Map(SavePage source, Models.Page destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.BookId = source.BookId;
            destination.Name = source.Name;
            destination.Description = source.Description;
        }
    }
}
