namespace Scenes.Service.IntegrationTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Scenes.Service.ViewModels;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Newtonsoft.Json;
    using Xunit;
    using Xunit.Abstractions;

    public class ScenesControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public ScenesControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_ScenesRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "scenes");

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[] { HttpMethods.Get, HttpMethods.Head, HttpMethods.Options, HttpMethods.Post },
                response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Options_ScenesWithId_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "scenes/1");

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(
                new string[]
                {
                    HttpMethods.Delete,
                    HttpMethods.Get,
                    HttpMethods.Head,
                    HttpMethods.Options,
                    HttpMethods.Patch,
                    HttpMethods.Post,
                    HttpMethods.Put,
                },
                response.Content.Headers.Allow);
        }

        [Fact]
        public async Task Delete_SceneFound_Returns204NoContentAsync()
        {
            var scene = new Models.Scene();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            this.SceneRepositoryMock.Setup(x => x.DeleteAsync(scene, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            var response = await this.client.DeleteAsync(new Uri("/scenes/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_SceneNotFound_Returns404NotFoundAsync()
        {
            this.SceneRepositoryMock.Setup(x => x.GetAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var response = await this.client.DeleteAsync(new Uri("/scenes/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_SceneFound_Returns200OkAsync()
        {
            var scene = new Models.Scene() { Modified = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client.GetAsync(new Uri("/scenes/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)), response.Content.Headers.LastModified);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
        }

        [Fact]
        public async Task Get_SceneNotFound_Returns404NotFoundAsync()
        {
            this.SceneRepositoryMock.Setup(x => x.GetAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var response = await this.client.GetAsync(new Uri("/scenes/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406NotAcceptableAsync()
        {
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);
            using var request = new HttpRequestMessage(HttpMethod.Get, "/scenes/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_SceneNotModifiedSince_Returns304NotModifiedAsync()
        {
            var scene = new Models.Scene() { Modified = new DateTimeOffset(2000, 1, 1, 23, 59, 59, TimeSpan.Zero) };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            using var request = new HttpRequestMessage(HttpMethod.Get, "/scenes/1");
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotModified, response.StatusCode);
        }

        [Fact]
        public async Task Get_SceneHasBeenModifiedSince_Returns200OKAsync()
        {
            var scene = new Models.Scene() { Modified = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            using var request = new HttpRequestMessage(HttpMethod.Get, "/scenes/1");
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]
        [InlineData("/scenes")]
        [InlineData("/scenes?first=3")]
        [InlineData("/scenes?first=3&after=THIS_IS_INVALID")]
        public async Task GetPage_FirstPage_Returns200OkAsync(string path)
        {
            var scenes = GetScenes();
            this.SceneRepositoryMock
                .Setup(x => x.GetScenesAsync(3, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Take(3).ToList());
            this.SceneRepositoryMock
                .Setup(x => x.GetTotalCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Count);
            this.SceneRepositoryMock
                .Setup(x => x.GetHasNextPageAsync(3, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var response = await this.client.GetAsync(new Uri(path, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            await this.AssertPageUrlsAsync(
                    response,
                    nextPageUrl: $"http://localhost/scenes?First=3&After={Cursor.ToCursor(new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero))}",
                    previousPageUrl: null,
                    expectedPageCount: 3,
                    actualPageCount: 3,
                    totalCount: 4)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task GetPage_SecondPage_Returns200OkAsync()
        {
            var scenes = GetScenes();
            this.SceneRepositoryMock
                .Setup(x => x.GetScenesAsync(3, new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Skip(3).Take(3).ToList());
            this.SceneRepositoryMock
                .Setup(x => x.GetTotalCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Count);
            this.SceneRepositoryMock
                .Setup(x => x.GetHasNextPageAsync(3, new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var response = await this.client
                .GetAsync(new Uri($"/scenes?First=3&After={Cursor.ToCursor(new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero))}", UriKind.Relative))
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            await this.AssertPageUrlsAsync(
                    response,
                    nextPageUrl: null,
                    previousPageUrl: $"http://localhost/scenes?Last=3&Before={Cursor.ToCursor(new DateTimeOffset(2000, 1, 4, 0, 0, 0, TimeSpan.Zero))}",
                    expectedPageCount: 3,
                    actualPageCount: 1,
                    totalCount: 4)
                .ConfigureAwait(false);
        }

        [Theory]
        [InlineData("/scenes?last=3")]
        [InlineData("/scenes?last=3&before=THIS_IS_INVALID")]
        public async Task GetPage_LastPage_Returns200OkAsync(string path)
        {
            var scenes = GetScenes();
            this.SceneRepositoryMock
                .Setup(x => x.GetScenesReverseAsync(3, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.TakeLast(3).ToList());
            this.SceneRepositoryMock
                .Setup(x => x.GetTotalCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Count);
            this.SceneRepositoryMock
                .Setup(x => x.GetHasPreviousPageAsync(3, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var response = await this.client.GetAsync(new Uri(path, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            await this.AssertPageUrlsAsync(
                    response,
                    nextPageUrl: null,
                    previousPageUrl: $"http://localhost/scenes?Last=3&Before={Cursor.ToCursor(new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero))}",
                    expectedPageCount: 3,
                    actualPageCount: 3,
                    totalCount: 4)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task GetPage_SecondLastPage_Returns200OkAsync()
        {
            var scenes = GetScenes();
            this.SceneRepositoryMock
                .Setup(x => x.GetScenesReverseAsync(3, null, new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero), It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.SkipLast(3).TakeLast(3).ToList());
            this.SceneRepositoryMock
                .Setup(x => x.GetTotalCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Count);
            this.SceneRepositoryMock
                .Setup(x => x.GetHasPreviousPageAsync(3, new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var response = await this.client
                .GetAsync(new Uri($"/scenes?Last=3&Before={Cursor.ToCursor(new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero))}", UriKind.Relative))
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            await this.AssertPageUrlsAsync(
                    response,
                    nextPageUrl: $"http://localhost/scenes?First=3&After={Cursor.ToCursor(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))}",
                    previousPageUrl: null,
                    expectedPageCount: 3,
                    actualPageCount: 1,
                    totalCount: 4)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task GetPage_MiddlePage_Returns200OkAsync()
        {
            var scenes = GetScenes();
            this.SceneRepositoryMock
                .Setup(x => x.GetScenesAsync(2, new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Skip(1).Take(2).ToList());
            this.SceneRepositoryMock
                .Setup(x => x.GetTotalCountAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(scenes.Count);
            this.SceneRepositoryMock
                .Setup(x => x.GetHasNextPageAsync(2, new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var response = await this.client
                .GetAsync(new Uri($"/scenes?First=2&After={Cursor.ToCursor(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero))}", UriKind.Relative))
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            await this.AssertPageUrlsAsync(
                    response,
                    nextPageUrl: $"http://localhost/scenes?First=2&After={Cursor.ToCursor(new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero))}",
                    previousPageUrl: $"http://localhost/scenes?Last=2&Before={Cursor.ToCursor(new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero))}",
                    expectedPageCount: 2,
                    actualPageCount: 2,
                    totalCount: 4)
                .ConfigureAwait(false);
        }

        [Fact]
        public async Task PostScene_Valid_Returns201CreatedAsync()
        {
            var saveScene = new SaveScene()
            {
                Name = "Test",
                Description = "Test",
            };
            var scene = new Models.Scene() { SceneId = 1 };
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Scene>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(scene);

            var response = await this.client.PostAsJsonAsync("scenes", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
            Assert.Equal(new Uri("http://localhost/scenes/1"), response.Headers.Location);
        }

        [Fact]
        public async Task PostScene_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("scenes", new SaveScene()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostScene_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "scenes")
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
            using var request = new HttpRequestMessage(HttpMethod.Post, "scenes")
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
            var saveScene = new SaveScene()
            {
                Name = "Test",
                Description = "Test",
            };
            var scene = new Models.Scene() { SceneId = 1 };
            this.SceneRepositoryMock
                .Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(scene);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock.Setup(x => x.UpdateAsync(scene, It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client.PutAsJsonAsync("scenes/1", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
        }

        [Fact]
        public async Task PutScene_SceneNotFound_Returns404NotFoundAsync()
        {
            var saveScene = new SaveScene()
            {
                Name = "Test",
                Description = "Test",
            };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var response = await this.client.PutAsJsonAsync("scenes/999", saveScene).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutScene_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("scenes/1", new SaveScene()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PatchScene_SceneNotFound_Returns404NotFoundAsync()
        {
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var content = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.SceneRepositoryMock.Setup(x => x.GetAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync(() => null);

            var response = await this.client
                .PatchAsync(new Uri("scenes/999", UriKind.Relative), content)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PatchScene_InvalidScene_Returns400BadRequestAsync()
        {
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var content = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            var scene = new Models.Scene();
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client
                .PatchAsync(new Uri("scenes/1", UriKind.Relative), content)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchScene_ValidScene_Returns200OkAsync()
        {
            var patch = new JsonPatchDocument<SaveScene>();
            patch.Add(x => x.Name, "Test");
            var json = JsonConvert.SerializeObject(patch);
            using var content = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            var scene = new Models.Scene() { SceneId = 1, Name = "Test", Description = "Test" };
            this.SceneRepositoryMock.Setup(x => x.GetAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(scene);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.SceneRepositoryMock.Setup(x => x.UpdateAsync(scene, It.IsAny<CancellationToken>())).ReturnsAsync(scene);

            var response = await this.client
                .PatchAsync(new Uri("scenes/1", UriKind.Relative), content)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var sceneViewModel = await response.Content.ReadAsAsync<Scene>(this.formatters).ConfigureAwait(false);
            Assert.Equal("Test", sceneViewModel.Name);
        }

        private static List<Models.Scene> GetScenes() =>
            new()
            {
                new Models.Scene() { Created = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero) },
                new Models.Scene() { Created = new DateTimeOffset(2000, 1, 2, 0, 0, 0, TimeSpan.Zero) },
                new Models.Scene() { Created = new DateTimeOffset(2000, 1, 3, 0, 0, 0, TimeSpan.Zero) },
                new Models.Scene() { Created = new DateTimeOffset(2000, 1, 4, 0, 0, 0, TimeSpan.Zero) },
            };

        private async Task AssertPageUrlsAsync(
            HttpResponseMessage response,
            string? nextPageUrl,
            string? previousPageUrl,
            int expectedPageCount,
            int actualPageCount,
            int totalCount)
        {
            var connection = await response.Content.ReadAsAsync<Connection<Scene>>(this.formatters).ConfigureAwait(false);

            Assert.Equal(actualPageCount, connection.Items.Count);
            Assert.Equal(actualPageCount, connection.PageInfo.Count);
            Assert.Equal(totalCount, connection.TotalCount);

            Assert.Equal(nextPageUrl is not null, connection.PageInfo.HasNextPage);
            Assert.Equal(previousPageUrl is not null, connection.PageInfo.HasPreviousPage);

            if (nextPageUrl is null)
            {
                Assert.Null(nextPageUrl);
            }
            else
            {
                Assert.Equal(new Uri(nextPageUrl), connection.PageInfo.NextPageUrl);
            }

            if (previousPageUrl is null)
            {
                Assert.Null(previousPageUrl);
            }
            else
            {
                Assert.Equal(new Uri(previousPageUrl), connection.PageInfo.PreviousPageUrl);
            }

            var firstPageUrl = $"http://localhost/scenes?First={expectedPageCount}";
            var lastPageUrl = $"http://localhost/scenes?Last={expectedPageCount}";

            Assert.Equal(new Uri(firstPageUrl), connection.PageInfo.FirstPageUrl);
            Assert.Equal(new Uri(lastPageUrl), connection.PageInfo.LastPageUrl);

            var linkValue = Assert.Single(response.Headers.GetValues("Link"));
            var expectedLink = $"<{firstPageUrl}>; rel=\"first\", <{lastPageUrl}>; rel=\"last\"";
            if (previousPageUrl is not null)
            {
                expectedLink = $"<{previousPageUrl}>; rel=\"previous\", " + expectedLink;
            }

            if (nextPageUrl is not null)
            {
                expectedLink = $"<{nextPageUrl}>; rel=\"next\", " + expectedLink;
            }

            Assert.Equal(expectedLink, linkValue);
        }
    }
}
