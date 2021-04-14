namespace Identity.Service.Controllers
{
    using System.Threading.Tasks;
    using Identity.Service.Constants;
    using Identity.Service.Services;
    using Identity.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;

    /// <summary>
    /// Auth controller.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [ApiVersion(ApiVersionName.V1)]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable CA1062 // Validate arguments of public methods
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
        /// Logs in a user.
        /// </summary>
        /// <param name="model">The login viewmodel.</param>
        /// <returns>A 200 OK response containing the newly created content or a 400 Bad Request if the content is
        /// invalid.</returns>
        [HttpPost("login", Name = AuthControllerRoute.Login)]
        [SwaggerResponse(StatusCodes.Status201Created, "The content was created.", typeof(OkObjectResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The content is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this.userService.LoginUserAsync(model).ConfigureAwait(false);
                if (result.IsSuccess)
                {
                    return new OkObjectResult(result);
                }

                return new BadRequestObjectResult(result);
            }

            return new BadRequestObjectResult("Some properties are not valid!");
        }

        /// <summary>
        /// Logs out a user.
        /// </summary>
        /// <returns>A 200 OK response containing the newly created content or a 400 Bad Request if the content is
        /// invalid.</returns>
        [HttpGet("logout", Name = AuthControllerRoute.Logout)]
        [SwaggerResponse(StatusCodes.Status201Created, "The content was created.", typeof(OkObjectResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The content is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await this.userService.LogoutUserAsync().ConfigureAwait(false);
            if (result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            return new BadRequestObjectResult("Some properties are not valid!");
        }

        /// <summary>
        /// Registers a user.
        /// </summary>
        /// <param name="model">The register viewmodel.</param>
        /// <returns>A 200 OK response containing the newly created content or a 400 Bad Request if the content is
        /// invalid.</returns>
        [HttpPost("register", Name = AuthControllerRoute.Register)]
        [SwaggerResponse(StatusCodes.Status201Created, "The content was created.", typeof(OkObjectResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The content is invalid.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The MIME type in the Accept HTTP header is not acceptable.", typeof(ProblemDetails))]
        [SwaggerResponse(StatusCodes.Status415UnsupportedMediaType, "The MIME type in the Content-Type HTTP header is unsupported.", typeof(ProblemDetails))]
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
#pragma warning restore CA1062 // Validate arguments of public methods
#pragma warning restore CA1822 // Mark members as static
}
