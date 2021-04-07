namespace Content.Service.IntegrationTest.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Content.Service.ViewModels;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Moq;
    using Xunit;
    using Xunit.Abstractions;

    public class ContentControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public ContentControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Delete_ContentFound_Returns204NoContentAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 1,
            };
            var content = new List<Models.Content> { new Models.Content() };
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            this.ContentRepositoryMock.Setup(x => x.DeleteAsync(content.First(), It.IsAny<CancellationToken>())).ReturnsAsync(0);
            var response = await this.client.DeleteAsync(new Uri("/content/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ContentNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 999,
            };
            var content = new List<Models.Content>();
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var response = await this.client.DeleteAsync(new Uri("/content/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_ContentFound_Returns200OkAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 1,
            };
            var content = new List<Models.Content> { new Models.Content { ContentId = 1 } };
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var contentViewModel = await response.Content.ReadAsAsync<List<Models.Content>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(content.First().ContentId, contentViewModel.First().ContentId);
        }

        [Fact]
        public async Task Get_ContentNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 999,
            };
            var content = new List<Models.Content>();
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406MethodNotAllowedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/content/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_ContentBySceneFound_Returns200OkAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                PageId = 1,
                BookId = 1,
            };
            var content = new List<Models.Content> { new Models.Content { ContentId = 1, ModifiedDate = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) } };
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var contentViewModel = await response.Content.ReadAsAsync<List<Content>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(content.First().ContentId, contentViewModel.First().ContentId);
        }

        [Fact]
        public async Task Get_ContentBySceneNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                PageId = 1,
                BookId = 999,
            };
            var content = new List<Models.Content>();
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeaderByScene_Returns406NotAcceptableAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                PageId = 1,
                BookId = 1,
            };
            var content = new List<Models.Content>();
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_ContentBySceneHasBeenModifiedSince_Returns200OKAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                PageId = 1,
                BookId = 1,
            };
            var content = new List<Models.Content> { new Models.Content() { ModifiedDate = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) } };
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);
            var uriString = AddQueryString("/content", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostContent_Valid_Returns201CreatedAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 1,
            };
            var saveContent = new SaveContent()
            {
                PageId = 1,
                BookId = 1,
                Value = "x",
            };
            var content = new Models.Content() { ContentId = 1 };
            this.ContentRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Content>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(content);

            var response = await this.client.PostAsJsonAsync("content", saveContent).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var contentViewModel = await response.Content.ReadAsAsync<Content>(this.formatters).ConfigureAwait(false);
            var uriString = AddQueryString("/content", filters);
            Assert.Equal(new Uri($"http://localhost{uriString}"), response.Headers.Location);
        }

        [Fact]
        public async Task PostContent_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("content", new SaveContent()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostContent_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "content")
            {
                Content = new ObjectContent<SaveContent>(null!, new JsonMediaTypeFormatter(), ContentType.Json),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostContent_UnsupportedMediaType_Returns415UnsupportedMediaTypeAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "content")
            {
                Content = new ObjectContent<SaveContent>(new SaveContent(), new JsonMediaTypeFormatter(), ContentType.Text),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, problemDetails.Status);
        }

        [Fact]
        public async Task PutContent_Valid_Returns200OkAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 1,
            };
            var saveContent = new SaveContent()
            {
                PageId = 1,
                BookId = 1,
                Value = "x",
            };
            var content = new List<Models.Content> { new Models.Content() { ContentId = 1 } };
            this.ContentRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(content);
            this.ContentRepositoryMock.Setup(x => x.UpdateAsync(content.First(), It.IsAny<CancellationToken>())).ReturnsAsync(content.First());

            var response = await this.client.PutAsJsonAsync("content/1", saveContent).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var contentViewModel = await response.Content.ReadAsAsync<Content>(this.formatters).ConfigureAwait(false);
            Assert.Equal(contentViewModel.ContentId, content.First().ContentId);
        }

        [Fact]
        public async Task PutContent_ContentNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.ContentOptionFilter
            {
                ContentId = 999,
            };
            var saveContent = new SaveContent()
            {
                PageId = 1,
                BookId = 1,
                Value = "x",
            };
            var content = new List<Models.Content>();
            this.ContentRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.ContentOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(content);

            var response = await this.client.PutAsJsonAsync("content/999", saveContent).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutContent_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("content/1", new SaveContent()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        private static string AddQueryString(string uriString, Models.ContentOptionFilter filters)
        {
            var provider = CultureInfo.InvariantCulture;

            if (filters.ContentId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"ContentId", filters.ContentId.Value.ToString(provider));
            }

            if (filters.PageId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"PageId", filters.PageId.Value.ToString(provider));
            }

            if (filters.BookId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"BookId", filters.BookId.Value.ToString(provider));
            }

            return uriString;
        }
    }
}
