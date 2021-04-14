namespace Identity.Service.Services
{
    using System.Threading.Tasks;
    using Identity.Service.ViewModels;

    /// <summary>
    /// Retrieves the user.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="model">The register view model.</param>
        /// <returns>A user mnager response.</returns>
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }
}
