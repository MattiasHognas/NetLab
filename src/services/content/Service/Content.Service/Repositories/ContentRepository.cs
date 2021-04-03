namespace Content.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Content.Service.Data;
    using Content.Service.Models;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Content repository.
    /// </summary>
    public class ContentRepository : IContentRepository
    {
        private readonly IDbContextFactory<ContentContext> contentContextFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentRepository"/> class.
        /// </summary>
        /// <param name="contentContextFactory">The content context factory.</param>
        public ContentRepository(IDbContextFactory<ContentContext> contentContextFactory) => this.contentContextFactory = contentContextFactory;

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        public async Task<Content> AddAsync(Content content, CancellationToken cancellationToken)
        {
            using (var context = this.contentContextFactory.CreateDbContext())
            {
                if (content is null)
                {
                    throw new ArgumentNullException(nameof(content));
                }

                context.Contents.Add(content);
                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return await Task.FromResult(content).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public async Task<int> DeleteAsync(Content content, CancellationToken cancellationToken)
        {
            using (var context = this.contentContextFactory.CreateDbContext())
            {
                if (context.Contents.Any(item => item.ContentId == content.ContentId))
                {
                    context.Contents.Remove(content);
                }

                return await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Get async.
        /// </summary>
        /// <param name="contentOptionFilter">The content option filter.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of content.</returns>
        public Task<List<Content>> GetAsync(
            ContentOptionFilter contentOptionFilter,
            CancellationToken cancellationToken) =>
            Task.FromResult(this.contentContextFactory.CreateDbContext()
                .Contents
                .OrderBy(x => x.ModifiedDate)
                .ThenBy(x => x.CreatedDate)
                .If<Models.Content>(contentOptionFilter.ContentId.HasValue, x => x.Where(y => y.ContentId == contentOptionFilter.ContentId))
                .If<Models.Content>(contentOptionFilter.PageId.HasValue, x => x.Where(y => y.PageId == contentOptionFilter.PageId))
                .If<Models.Content>(contentOptionFilter.BookId.HasValue, x => x.Where(y => y.BookId == contentOptionFilter.BookId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(this.contentContextFactory.CreateDbContext().Contents.Count());

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        public async Task<Content> UpdateAsync(Content content, CancellationToken cancellationToken)
        {
            using (var context = this.contentContextFactory.CreateDbContext())
            {
                if (content is null)
                {
                    throw new ArgumentNullException(nameof(content));
                }

                var existingContent = context.Contents.FirstOrDefault(x => x.ContentId == content.ContentId);
                if (existingContent is null)
                {
                    throw new NotSupportedException($"The content does not exist in the database {nameof(content)}");
                }

                existingContent.Value = content.Value;

                await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return await Task.FromResult(content).ConfigureAwait(false);
            }
        }
    }
}
