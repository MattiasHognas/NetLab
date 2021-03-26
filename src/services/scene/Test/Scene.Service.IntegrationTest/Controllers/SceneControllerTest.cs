namespace Scene.Service.IntegrationTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Moq;
    using Newtonsoft.Json;
    using Scene.Service.ViewModels;
    using Xunit;
    using Xunit.Abstractions;

    public class SceneControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public SceneControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_SceneRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "scene/1");

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[]
                {
                    HttpMethods.Delete,
                    HttpMethods.Head,
                    HttpMethods.Options,
                    HttpMethods.Patch,
                    HttpMethods.Put,
                },
                response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Options_Filter_Returns200OkAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var uriString = AddQueryString("scene", filters);
            using var request = new HttpRequestMessage(HttpMethod.Options, uriString);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[]
                {
                    HttpMethods.Get,
                    HttpMethods.Post,
                    HttpMethods.Head,
                    HttpMethods.Options,
                },
                response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Options_SceneId_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "scene/1");

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[]
                {
                    HttpMethods.Delete,
                    HttpMethods.Head,
                    HttpMethods.Options,
                    HttpMethods.Patch,
                    HttpMethods.Put,
                },
                response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Delete_SceneFound_Returns204NoSceneAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene> { new Models.Scene() };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            this.SceneRepositoryMock.Setup(x => x.DeleteAsync(scene.First(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var response = await this.client.DeleteAsync(new Uri("/scene/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_SceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 999,
            };
            var scene = new List<Models.Scene>();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var response = await this.client.DeleteAsync(new Uri("/scene/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_SceneFound_Returns200OkAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene> { new Models.Scene { SceneId = 1 } };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<List<Models.Scene>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(scene.First().SceneId, sceneViewModel.First().SceneId);
        }

        [Fact]
        public async Task Get_SceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 999,
            };
            var scene = new List<Models.Scene>();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406MethodNotAllowedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/scene/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_SceneBySceneFound_Returns200OkAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene> { new Models.Scene { SceneId = 1, Modified = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) } };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<List<Scene>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(scene.First().SceneId, sceneViewModel.First().SceneId);
        }

        [Fact]
        public async Task Get_SceneBySceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 999,
            };
            var scene = new List<Models.Scene>();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeaderByScene_Returns406NotAcceptableAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene>();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_SceneBySceneHasBeenModifiedSince_Returns200OKAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene> { new Models.Scene() { Modified = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) } };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            var uriString = AddQueryString("/scene", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostScene_Valid_Returns201CreatedAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var saveScene = new SaveScene()
            {
                SceneId = 1,
                X1 = 1,
                X2 = 2,
                Y1 = 1,
                Y2 = 2,
                UserId = 1,
            };
            var scene = new Models.Scene() { SceneId = 1 };
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Scene>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(scene);

            var response = await this.client.PostAsJsonAsync("scene", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
            var uriString = AddQueryString("/scene", filters);
            Assert.Equal(new Uri($"http://localhost{uriString}"), response.Headers.Location);
        }

        [Fact]
        public async Task PostScene_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("scene", new SaveScene()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostScene_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "scene")
            {
                Content = new ObjectContent<SaveScene>(null!, new JsonMediaTypeFormatter(), ContentType.Json),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostScene_UnsupportedMediaType_Returns415UnsupportedMediaTypeAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "scene")
            {
                Content = new ObjectContent<SaveScene>(new SaveScene(), new JsonMediaTypeFormatter(), ContentType.Text),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, problemDetails.Status);
        }

        [Fact]
        public async Task PutScene_Valid_Returns200OkAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var saveScene = new SaveScene()
            {
                SceneId = 1,
                X1 = 1,
                X2 = 2,
                Y1 = 1,
                Y2 = 2,
                UserId = 1,
            };
            var scene = new List<Models.Scene> { new Models.Scene() { SceneId = 1 } };
            this.SceneRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(scene);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock.Setup(x => x.UpdateAsync(scene.First(), It.IsAny<CancellationToken>())).ReturnsAsync(scene.First());

            var response = await this.client.PutAsJsonAsync("scene/1", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
            Assert.Equal(sceneViewModel.SceneId, scene.First().SceneId);
        }

        [Fact]
        public async Task PutScene_SceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 999,
            };
            var saveScene = new SaveScene()
            {
                SceneId = 1,
                X1 = 1,
                X2 = 2,
                Y1 = 1,
                Y2 = 2,
                UserId = 1,
            };
            var scene = new List<Models.Scene>();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client.PutAsJsonAsync("scene/999", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutScene_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("scene/1", new SaveScene()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PatchScene_SceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 999,
            };
            var scene = new List<Models.Scene>();
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Remove(x => x.X1);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client
                .PatchAsync(new Uri("scene/999", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PatchScene_InvalidScene_Returns400BadRequestAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var scene = new List<Models.Scene>
            {
                new Models.Scene { SceneId = 1 },
            };
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Remove(x => x.X1);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client
                .PatchAsync(new Uri("scene/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchScene_ValidScene_Returns200OkAsync()
        {
            var filters = new Models.SceneOptionFilter
            {
                SceneId = 1,
            };
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Add(x => x.X1, 2);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            var scene = new List<Models.Scene> { new Models.Scene() { ContentId = 1, SceneId = 1, X1 = 1, X2 = 2, Y1 = 1, Y2 = 2, UserId = 1 } };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.SceneOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock.Setup(x => x.UpdateAsync(scene.First(), It.IsAny<CancellationToken>())).ReturnsAsync(scene.First());

            var response = await this.client
                .PatchAsync(new Uri("scene/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
            Assert.Equal(2, sceneViewModel.X1);
        }

        private static string AddQueryString(string uriString, Models.SceneOptionFilter filters)
        {
            var provider = CultureInfo.InvariantCulture;

            if (filters.ContentId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"ContentId", filters.ContentId.Value.ToString(provider));
            }

            if (filters.SceneId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"SceneId", filters.SceneId.Value.ToString(provider));
            }

            return uriString;
        }
    }
}
