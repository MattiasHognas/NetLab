namespace Identity.Service.Repositories
{
    using Identity.Service.Data;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Auth repository.
    /// </summary>
    public class AuthRepository : IAuthRepository
    {
#pragma warning disable IDE0052
        private readonly IDbContextFactory<AppDbContext> appContextFactory;
#pragma warning restore IDE0052

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthRepository"/> class.
        /// </summary>
        /// <param name="appContextFactory">The content context factory.</param>
        public AuthRepository(IDbContextFactory<AppDbContext> appContextFactory) => this.appContextFactory = appContextFactory;
    }
}
