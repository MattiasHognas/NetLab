namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.Services;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for book viewmodel to save book model.
    /// </summary>
    public class BookToSaveBookMapper : IMapper<Models.Book, SaveBook>, IMapper<SaveBook, Models.Book>
    {
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BookToSaveBookMapper"/> class.
        /// </summary>
        /// <param name="clockService">The clock service.</param>
        public BookToSaveBookMapper(IClockService clockService) =>
            this.clockService = clockService;

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

            destination.BookId = source.BookId;
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

            var now = this.clockService.UtcNow;

            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }

            destination.BookId = source.BookId;
            destination.Name = source.Name;
            destination.Description = source.Description;
            destination.Modified = now;
        }
    }
}
