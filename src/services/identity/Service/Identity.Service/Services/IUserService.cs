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
        /// Logs in the user.
        /// </summary>
        /// <param name="model">The login view model.</param>
        /// <returns>A user mnager response.</returns>
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns>A user mnager response.</returns>
        Task<UserManagerResponse> LogoutUserAsync();

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="model">The register view model.</param>
        /// <returns>A user mnager response.</returns>
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }
}
