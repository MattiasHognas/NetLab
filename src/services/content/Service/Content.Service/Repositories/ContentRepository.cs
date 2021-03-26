namespace Content.Service.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Content.Service.Models;

    /// <summary>
    /// Content repository.
    /// </summary>
    public class ContentRepository : IContentRepository
    {
        private static readonly List<Content> Content = new()
        {
            new Content()
            {
                ContentId = 1,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 1,
                X2 = 2,
                Y1 = 1,
                Y2 = 2,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Content()
            {
                ContentId = 2,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 2,
                X2 = 3,
                Y1 = 2,
                Y2 = 3,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
            new Content()
            {
                ContentId = 3,
                SceneId = 1,
                Created = DateTimeOffset.UtcNow.AddDays(-8),
                X1 = 3,
                X2 = 4,
                Y1 = 3,
                Y2 = 4,
                UserId = 1,
                Modified = DateTimeOffset.UtcNow.AddDays(-8),
            },
        };

        /// <summary>
        /// Add async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        public Task<Content> AddAsync(Content content, CancellationToken cancellationToken)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            Content.Add(content);
            content.ContentId = Content.Max(x => x.ContentId) + 1;
            return Task.FromResult(content);
        }

        /// <summary>
        /// Delete async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A completed task.</returns>
        public Task DeleteAsync(Content content, CancellationToken cancellationToken)
        {
            if (Content.Contains(content))
            {
                Content.Remove(content);
            }

            return Task.CompletedTask;
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
            Task.FromResult(Content
                .OrderBy(x => x.Created)
                .If(contentOptionFilter.ContentId.HasValue, x => x.Where(y => y.ContentId == contentOptionFilter.ContentId))
                .If(contentOptionFilter.SceneId.HasValue, x => x.Where(y => y.SceneId == contentOptionFilter.SceneId))
                .ToList());

        /// <summary>
        /// Get total count async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An id.</returns>
        public Task<int> GetTotalCountAsync(CancellationToken cancellationToken) => Task.FromResult(Content.Count);

        /// <summary>
        /// Update async.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A content.</returns>
        public Task<Content> UpdateAsync(Content content, CancellationToken cancellationToken)
        {
            if (content is null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            var existingContent = Content.First(x => x.ContentId == content.ContentId);
            // existingContent.SceneId = content.SceneId;
            existingContent.X1 = content.X1;
            existingContent.X2 = content.X2;
            existingContent.Y1 = content.Y1;
            existingContent.Y2 = content.Y2;
            // existingContent.UserId = content.UserId;
            return Task.FromResult(content);
        }
    }
}
