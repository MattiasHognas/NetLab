namespace Identity.Service.Data
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Identity.Service.Models;
    using Identity.Service.Services;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// App context.
    /// </summary>
    public class AppDbContext : IdentityDbContext
    {
        private readonly IPrincipalService principalService;
        private readonly IClockService clockService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The workspace context options.</param>
        /// <param name="principalService">The principal service.</param>
        /// <param name="clockService">The clock service.</param>
        public AppDbContext(
            DbContextOptions options,
            IPrincipalService principalService,
            IClockService clockService)
            : base(options)
        {
            this.principalService = principalService;
            this.clockService = clockService;
        }

        /// <summary>
        /// Gets the member.
        /// </summary>
        public DbSet<Models.Member> Member => this.Set<Models.Member>();

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
        /// <param name="builder">The model builder.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Member>()
                .HasKey(b => b.MemberId);
        }

        private void AddTimestamps()
        {
            var entities = this.ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            var currentDate = this.clockService.UtcNow;
            var nameIdentifier = this.principalService.NameIdentifier;
            var currentUserId = !string.IsNullOrEmpty(nameIdentifier)
                ? Convert.ToInt64(nameIdentifier, CultureInfo.InvariantCulture)
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
