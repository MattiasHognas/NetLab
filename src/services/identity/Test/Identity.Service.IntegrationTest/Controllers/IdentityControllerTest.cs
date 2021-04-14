namespace Identity.Service.IntegrationTest.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Identity.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class IdentityControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public IdentityControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Login_ModelValidationFail_Returns400BadRequestAsync()
        {
            var loginViewModel = new ViewModels.LoginViewModel()
            {
                Email = "x",
                Password = "y",
            };

            var response = await this.client.PostAsJsonAsync("auth/login", loginViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task Login_ServiceFail_Returns400BadRequestAsync()
        {
            var loginViewModel = new ViewModels.LoginViewModel()
            {
                Email = "xx@xx.xx",
                Password = "yyyyyy",
            };
            this.UserServiceMock
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = false });

            var response = await this.client.PostAsJsonAsync("auth/login", loginViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var userManagerResponse = await response.Content.ReadAsAsync<UserManagerResponse>(this.formatters).ConfigureAwait(false);
            Assert.False(userManagerResponse.IsSuccess);
        }

        [Fact]
        public async Task Login_Valid_Returns200OKAsync()
        {
            var loginViewModel = new ViewModels.LoginViewModel()
            {
                Email = "xx@xx.xx",
                Password = "yyyyyy",
            };
            this.UserServiceMock
                .Setup(x => x.LoginUserAsync(It.IsAny<LoginViewModel>()))
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = true });

            var response = await this.client.PostAsJsonAsync("auth/login", loginViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var userManagerResponse = await response.Content.ReadAsAsync<ViewModels.UserManagerResponse>(this.formatters).ConfigureAwait(false);
            Assert.True(userManagerResponse.IsSuccess);
        }

        [Fact]
        public async Task Logout_ServiceFail_Returns400BadRequestAsync()
        {
            this.UserServiceMock
                .Setup(x => x.LogoutUserAsync())
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = false });

            var response = await this.client.GetAsync("auth/logout").ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<string>(this.formatters).ConfigureAwait(false);
            Assert.Equal("Some properties are not valid!", problemDetails);
        }

        [Fact]
        public async Task Logout_Valid_Returns200OKAsync()
        {
            this.UserServiceMock
                .Setup(x => x.LogoutUserAsync())
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = true });

            var response = await this.client.GetAsync("auth/logout").ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var userManagerResponse = await response.Content.ReadAsAsync<ViewModels.UserManagerResponse>(this.formatters).ConfigureAwait(false);
            Assert.True(userManagerResponse.IsSuccess);
        }

        [Fact]
        public async Task Register_ModelValidationFail_Returns400BadRequestAsync()
        {
            var registerViewModel = new ViewModels.RegisterViewModel()
            {
                Email = "x",
                Password = "y",
                ConfirmPassword = "z",
            };

            var response = await this.client.PostAsJsonAsync("auth/register", registerViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task Register_ServiceFail_Returns400BadRequestAsync()
        {
            var registerViewModel = new ViewModels.RegisterViewModel()
            {
                Email = "xx@xx.xx",
                Password = "yyyyyy",
                ConfirmPassword = "yyyyyy",
            };
            this.UserServiceMock
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterViewModel>()))
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = false });

            var response = await this.client.PostAsJsonAsync("auth/register", registerViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var userManagerResponse = await response.Content.ReadAsAsync<UserManagerResponse>(this.formatters).ConfigureAwait(false);
            Assert.False(userManagerResponse.IsSuccess);
        }

        [Fact]
        public async Task Register_Valid_Returns200OKAsync()
        {
            var registerViewModel = new ViewModels.RegisterViewModel()
            {
                Email = "xx@xx.xx",
                Password = "yyyyyy",
                ConfirmPassword = "yyyyyy",
            };
            this.UserServiceMock
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterViewModel>()))
                .ReturnsAsync(new ViewModels.UserManagerResponse() { IsSuccess = true });

            var response = await this.client.PostAsJsonAsync("auth/register", registerViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var userManagerResponse = await response.Content.ReadAsAsync<ViewModels.UserManagerResponse>(this.formatters).ConfigureAwait(false);
            Assert.True(userManagerResponse.IsSuccess);
        }
    }
}
