namespace Workspace.Service.Mappers
{
    using System;
    using Boxed.Mapping;
    using Workspace.Service.ViewModels;

    /// <summary>
    /// Mapper for page option filter viewmodel to page option filter model.
    /// </summary>
    public class PageOptionFilterToPageOptionFilterMapper : IMapper<PageOptionFilter, Models.PageOptionFilter>
    {
        /// <summary>
        /// Map page option filter viewmodel to page option filter model.
        /// </summary>
        /// <param name="source">The page option filter viewmodel.</param>
        /// <param name="destination">The page option filter model.</param>
        public void Map(PageOptionFilter source, Models.PageOptionFilter destination)
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
        }
    }
}
