namespace Workspace.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Workspace.Service.Models;

    /// <summary>
    /// Page repository.
    /// </summary>
    public class PageRepository : IPageRepository
    {
        private static readonly List<Page> Page = new()
        {
            new Page()
            {
                PageId = 1,
                BookId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "A",
                Description = "A",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Page()
            {
                PageId = 2,
                BookId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "B",
                Description = "B",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Page()
            {
                PageId = 3,
                BookId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                Name = "C",
                Description = "C",
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
        };

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        public Task<Page> AddAsync(Page page, CancellationToken cancellationToken)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            Page.Add(page);
            page.PageId = Page.Max(x => x.PageId) + 1;
            return Task.FromResult(page);
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Page page, CancellationToken cancellationToken)
        {
            if (Page.Contains(page))
            {
                Page.Remove(page);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="pageOptionFilter">The page option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of page.</returns>
        public Task<List<Page>> GetAsync(
            PageOptionFilter pageOptionFilter,
            CancellationToken cancellationToken) =>
            Task.FromResult(Page
                .OrderBy(x => x.Created)
                .If(pageOptionFilter.PageId.HasValue, x => x.Where(y => y.PageId == pageOptionFilter.PageId))
                .If(pageOptionFilter.BookId.HasValue, x => x.Where(y => y.BookId == pageOptionFilter.BookId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Page.Count);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        public Task<Page> UpdateAsync(Page page, CancellationToken cancellationToken)
        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            var existingPage = Page.First(x => x.PageId == page.PageId);
            existingPage.Name = page.Name;
            existingPage.Description = page.Description;
            return Task.FromResult(page);
        }
    }
}
