namespace Content.Service.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using Content.Service.Models;
    using Content.Service.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Content context.
    /// </summary>
    public class ContentContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentContext"/> class.
        /// </summary>
        /// <param name="options">The content context options.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="clockService">The clock service.</param>
        public ContentContext(
            DbContextOptions<ContentContext> options,
            IHttpContextAccessor httpContextAccessor,
            IClockService clockService)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.clockService = clockService;
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public DbSet<Models.Content> Contents => this.Set<Models.Content>();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public override int SaveChanges()
        {
            this.AddTimestamps();
            return base.SaveChanges();
        }

        private void AddTimestamps()
        {
            var entities = this.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentDate = this.clockService.UtcNow;
            var currentUserId = !string.IsNullOrEmpty(userId)
                ? Convert.ToUInt64(userId, CultureInfo.InvariantCulture)
                : 0;

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedDate = currentDate;
                    ((BaseEntity)entity.Entity).CreatedBy = currentUserId;
                }

                ((BaseEntity)entity.Entity).ModifiedDate = currentDate;
                ((BaseEntity)entity.Entity).ModifiedBy = currentUserId;
            }
        }
    }
}
