namespace Workspace.Service.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Workspace.Service.Models;
    using Workspace.Service.Services;

    /// <summary>
    /// Workspace context.
    /// </summary>
    public class WorkspaceContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceContext"/> class.
        /// </summary>
        /// <param name="options">The workspace context options.</param>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        /// <param name="clockService">The clock service.</param>
        public WorkspaceContext(
            DbContextOptions<WorkspaceContext> options,
            IHttpContextAccessor httpContextAccessor,
            IClockService clockService)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.clockService = clockService;
        }

        /// <summary>
        /// Gets the books.
        /// </summary>
        public DbSet<Models.Book> Books => this.Set<Models.Book>();

        /// <summary>
        /// Gets the pages.
        /// </summary>
        public DbSet<Models.Page> Pages => this.Set<Models.Page>();

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public override int SaveChanges()
        {
            this.AddTimestamps();
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.Entity<Page>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Pages);

        private void AddTimestamps()
        {
            var entities = this.ChangeTracker
                .Entries()
                .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

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
