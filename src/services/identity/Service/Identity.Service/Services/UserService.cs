namespace Identity.Service.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Identity.Service.ViewModels;
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// Retrieves the user.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
#pragma warning disable IDE0052
        private readonly RoleManager<IdentityRole> roleManager;
#pragma warning restore IDE0052

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        public UserService(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Log in the user.
        /// </summary>
        /// <param name="model">The log in view model.</param>
        /// <returns>A user mnager response.</returns>
        public async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            var user = await this.userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "Could not log in user",
                    IsSuccess = false,
                };
            }

            var result = await this.signInManager.PasswordSignInAsync(user, model.Password, true, false).ConfigureAwait(false);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User signed in successfully",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "Could not log in user",
                IsSuccess = false,
            };
        }

        /// <summary>
        /// Logs out the user.
        /// </summary>
        /// <returns>A user mnager response.</returns>
        public async Task<UserManagerResponse> LogoutUserAsync()
        {
            await this.signInManager.SignOutAsync().ConfigureAwait(false);
            return new UserManagerResponse
            {
                Message = "User logged out successfully",
                IsSuccess = true,
            };
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="model">The register view model.</param>
        /// <returns>A user mnager response.</returns>
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password doesn't match the entered password!",
                    IsSuccess = false,
                };
            }

            var identityUser = new IdentityUser
            {
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await this.userManager.CreateAsync(identityUser, model.Password).ConfigureAwait(false);

            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User created successfully",
                    IsSuccess = true,
                };
            }

            return new UserManagerResponse
            {
                Message = "Could not create user",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description),
            };
        }
    }
}
