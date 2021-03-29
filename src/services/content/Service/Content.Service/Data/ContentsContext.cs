namespace Content.Service.Data
{
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Content context.
    /// </summary>
    public class ContentsContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentsContext"/> class.
        /// </summary>
        /// <param name="options">The content context options.</param>
        public ContentsContext(DbContextOptions<ContentsContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        public DbSet<Models.Content> Content => this.Set<Models.Content>();
    }
}
