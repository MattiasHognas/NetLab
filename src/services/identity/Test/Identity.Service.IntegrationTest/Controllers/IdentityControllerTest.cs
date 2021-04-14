namespace Identity.Service.IntegrationTest.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Identity.Service.ViewModels;
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
        public async Task Register_Valid_Returns201CreatedAsync()
        {
            var registerViewModel = new ViewModels.RegisterViewModel()
            {
                Email = "xx@xx.xx",
                Password = "yyyyyy",
                ConfirmPassword = "yyyyyy",
            };
            var userManagerResponse = new ViewModels.UserManagerResponse() { IsSuccess = true };
            this.UserServiceMock
                .Setup(x => x.RegisterUserAsync(It.IsAny<RegisterViewModel>()))
                .ReturnsAsync(userManagerResponse);

            var response = await this.client.PostAsJsonAsync("auth/register", registerViewModel).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var contentViewModel = await response.Content.ReadAsAsync<ViewModels.UserManagerResponse>(this.formatters).ConfigureAwait(false);
        }
    }
}
