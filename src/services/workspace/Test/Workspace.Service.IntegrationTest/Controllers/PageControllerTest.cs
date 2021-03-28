namespace Workspace.Service.IntegrationTest.Controllers
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
    using Workspace.Service.ViewModels;
    using Xunit;
    using Xunit.Abstractions;

    public class PageControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public PageControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_PageRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "page/1");

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
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var uriString = AddQueryString("page", filters);
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
        public async Task Options_PageId_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "page/1");

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
        public async Task Delete_PageFound_Returns204NoPageAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page> { new Models.Page() };
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            this.PageRepositoryMock.Setup(x => x.DeleteAsync(page.First(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var response = await this.client.DeleteAsync(new Uri("/page/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_PageNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 999,
            };
            var page = new List<Models.Page>();
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var response = await this.client.DeleteAsync(new Uri("/page/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_PageFound_Returns200OkAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page> { new Models.Page { PageId = 1 } };
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var pageViewModel = await response.Content.ReadAsAsync<List<Models.Page>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(page.First().PageId, pageViewModel.First().PageId);
        }

        [Fact]
        public async Task Get_PageNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 999,
            };
            var page = new List<Models.Page>();
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406MethodNotAllowedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/page/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_PageByPageFound_Returns200OkAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page> { new Models.Page { PageId = 1, Modified = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) } };
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var pageViewModel = await response.Content.ReadAsAsync<List<Page>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(page.First().PageId, pageViewModel.First().PageId);
        }

        [Fact]
        public async Task Get_PageByPageNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 999,
            };
            var page = new List<Models.Page>();
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeaderByPage_Returns406NotAcceptableAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page>();
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_PageByPageHasBeenModifiedSince_Returns200OKAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page> { new Models.Page() { Modified = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) } };
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            var uriString = AddQueryString("/page", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostPage_Valid_Returns201CreatedAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var savePage = new SavePage()
            {
                PageId = 1,
                Name = "x",
                Description = "x",
            };
            var page = new Models.Page() { PageId = 1 };
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.PageRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Page>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            var response = await this.client.PostAsJsonAsync("page", savePage).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var pageViewModel = await response.Content.ReadAsAsync<Page>(this.formatters).ConfigureAwait(false);
            var uriString = AddQueryString("/page", filters);
            Assert.Equal(new Uri($"http://localhost{uriString}"), response.Headers.Location);
        }

        [Fact]
        public async Task PostPage_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("page", new SavePage()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostPage_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "page")
            {
                Content = new ObjectContent<SavePage>(null!, new JsonMediaTypeFormatter(), ContentType.Json),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostPage_UnsupportedMediaType_Returns415UnsupportedMediaTypeAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "page")
            {
                Content = new ObjectContent<SavePage>(new SavePage(), new JsonMediaTypeFormatter(), ContentType.Text),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, problemDetails.Status);
        }

        [Fact]
        public async Task PutPage_Valid_Returns200OkAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var savePage = new SavePage()
            {
                PageId = 1,
                Name = "x",
                Description = "x",
            };
            var page = new List<Models.Page> { new Models.Page() { PageId = 1 } };
            this.PageRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.PageRepositoryMock.Setup(x => x.UpdateAsync(page.First(), It.IsAny<CancellationToken>())).ReturnsAsync(page.First());

            var response = await this.client.PutAsJsonAsync("page/1", savePage).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var pageViewModel = await response.Content.ReadAsAsync<Page>(this.formatters).ConfigureAwait(false);
            Assert.Equal(pageViewModel.PageId, page.First().PageId);
        }

        [Fact]
        public async Task PutPage_PageNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 999,
            };
            var savePage = new SavePage()
            {
                PageId = 1,
                Name = "x",
                Description = "x",
            };
            var page = new List<Models.Page>();
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);

            var response = await this.client.PutAsJsonAsync("page/999", savePage).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutPage_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("page/1", new SavePage()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PatchPage_PageNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 999,
            };
            var page = new List<Models.Page>();
            var patch = new JsonPatchDocument<SavePage>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);

            var response = await this.client
                .PatchAsync(new Uri("page/999", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PatchPage_InvalidPage_Returns400BadRequestAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var page = new List<Models.Page>
            {
                new Models.Page { PageId = 1 },
            };
            var patch = new JsonPatchDocument<SavePage>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);

            var response = await this.client
                .PatchAsync(new Uri("page/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchPage_ValidPage_Returns200OkAsync()
        {
            var filters = new Models.PageOptionFilter
            {
                PageId = 1,
            };
            var patch = new JsonPatchDocument<SavePage>();
            patch.Add(x => x.Name, "x");
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            var page = new List<Models.Page> { new Models.Page() { PageId = 1, Name = "x", Description = "x" } };
            this.PageRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.PageOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(page);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.PageRepositoryMock.Setup(x => x.UpdateAsync(page.First(), It.IsAny<CancellationToken>())).ReturnsAsync(page.First());

            var response = await this.client
                .PatchAsync(new Uri("page/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var pageViewModel = await response.Content.ReadAsAsync<Page>(this.formatters).ConfigureAwait(false);
            Assert.Equal("x", pageViewModel.Name);
        }

        private static string AddQueryString(string uriString, Models.PageOptionFilter filters)
        {
            var provider = CultureInfo.InvariantCulture;

            if (filters.PageId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"PageId", filters.PageId.Value.ToString(provider));
            }

            return uriString;
        }
    }
}
