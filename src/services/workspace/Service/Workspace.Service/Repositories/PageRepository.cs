namespace Workspace.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Workspace.Service.Data;
    using Workspace.Service.Models;

    /// <summary>
    /// Page repository.
    /// </summary>
    public class PageRepository : IPageRepository
    {
        private readonly IDbContextFactory<WorkspaceContext> workspaceContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepository"/> class.
        /// </summary>
        /// <param name="workspaceContextFactory">The content context factory.</param>
        public PageRepository(IDbContextFactory<WorkspaceContext> workspaceContextFactory) => this.workspaceContextFactory = workspaceContextFactory;

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        public async Task<Page> AddAsync(Page page, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (page is null)
                {
                    throw new ArgumentNullException(nameof(page));
                }

                context.Pages.Add(page);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return page;
            }
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public async Task DeleteAsync(Page page, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (context.Pages.Contains(page))
                {
                    context.Pages.Remove(page);
                }

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
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
            Task.FromResult(this.workspaceContextFactory.CreateDbContext()
                .Pages
                .OrderBy(x => x.ModifiedDate)
                .ThenBy(x => x.CreatedDate)
                .If(pageOptionFilter.PageId.HasValue, x => x.Where(y => y.PageId == pageOptionFilter.PageId))
                .If(pageOptionFilter.BookId.HasValue, x => x.Where(y => y.BookId == pageOptionFilter.BookId))
                .ToList());

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A page.</returns>
        public async Task<Page> UpdateAsync(Page page, CancellationToken cancellationToken)
        {
            using (var context = this.workspaceContextFactory.CreateDbContext())
            {
                if (page is null)
                {
                    throw new ArgumentNullException(nameof(page));
                }

                var existingPage = context.Pages.First(x => x.PageId == page.PageId);
                existingPage.Name = page.Name;
                existingPage.Description = page.Description;
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return existingPage;
            }
        }
    }
}
