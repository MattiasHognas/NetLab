namespace Content.Service.Services
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Retrieves the current user. Helps with unit testing by letting you mock the user.
    /// </summary>
    public class PrincipalService : IPrincipalService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrincipalService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The http context accessor.</param>
        public PrincipalService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        /// <summary>
        /// Gets the name identifier.
        /// </summary>
        public string NameIdentifier => this.httpContextAccessor.HttpContext!.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    }
}
