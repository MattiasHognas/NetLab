namespace Identity.Service.Controllers
{
    using System.Threading.Tasks;
    using Identity.Service.Services;
    using Identity.Service.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Auth controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userService">The user service.</param>
        public AuthController(IUserService userService) =>
            this.userService = userService;

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>A 204 NoContent response if the book was deleted or a 404 Not Found if a book with the specified
        /// id was not found.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.userService.RegisterUserAsync(model).ConfigureAwait(false);
                if (result.IsSuccess)
                {
                    return new OkObjectResult(result);
                }

                return new BadRequestObjectResult(result);
            }

            return new BadRequestObjectResult("Some properties are not valid!");
        }
    }
}
