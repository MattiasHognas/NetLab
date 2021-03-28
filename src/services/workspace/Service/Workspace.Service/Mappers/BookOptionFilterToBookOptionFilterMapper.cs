namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for book option filter viewmodel to book option filter model.
    /// </summary>
    public class BookOptionFilterToBookOptionFilterMapper : IMapper<BookOptionFilter, Models.BookOptionFilter>
    {
        /// <summary>
        /// Map book option filter viewmodel to book option filter model.
        /// </summary>
        /// <param name="source">The book option filter viewmodel.</param>
        /// <param name="destination">The book option filter model.</param>
        public void Map(BookOptionFilter source, Models.BookOptionFilter destination)
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
        }
    }
}
