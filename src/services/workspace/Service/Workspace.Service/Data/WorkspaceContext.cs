namespace Workspace.Service.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
    using Workspace.Service.Models;
    using Workspace.Service.Services;

    /// <summary>
    /// Workspace context.
    /// </summary>
    public class WorkspaceContext : DbContext
    {
        private readonly IPrincipalService principalService;
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkspaceContext"/> class.
        /// </summary>
        /// <param name="options">The workspace context options.</param>
        /// <param name="principalService">The principal service.</param>
        /// <param name="clockService">The clock service.</param>
        public WorkspaceContext(
            DbContextOptions<WorkspaceContext> options,
            IPrincipalService principalService,
            IClockService clockService)
            : base(options)
        {
            this.principalService = principalService;
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
        /// <returns>The result.</returns>
        public override int SaveChanges()
        {
            this.AddTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Accept all changes on success.</param>
        /// <returns>The result.</returns>
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        /// <summary>
        /// Saves the changes async.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.AddTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Event on model creating.
        /// </summary>
        /// <param name="modelBuilder">The model binder.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasKey(b => b.BookId);
            modelBuilder.Entity<Book>()
                .HasMany(b => b.Pages)
                .WithOne(p => p.Book)
                .HasForeignKey(p => p.BookId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Page>()
                .HasKey(p => p.PageId);
            modelBuilder.Entity<Page>()
                .HasOne(p => p.Book)
                .WithMany(b => b.Pages);

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
                // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
                // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
                // use the DateTimeOffsetToBinaryConverter
                // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
                // This only supports millisecond precision, but should be sufficient for most use cases.
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                                || p.PropertyType == typeof(DateTimeOffset?));
                    foreach (var property in properties)
                    {
                        modelBuilder
                            .Entity(entityType.Name)
                            .Property(property.Name)
                            .HasConversion(new DateTimeOffsetToBinaryConverter());
                    }
                }
            }
        }

        private void AddTimestamps()
        {
            var entities = this.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var currentDate = this.clockService.UtcNow;
            var nameIdentifier = this.principalService.NameIdentifier;
            var currentUserId = !string.IsNullOrEmpty(nameIdentifier)
                ? Convert.ToUInt64(nameIdentifier, CultureInfo.InvariantCulture)
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
