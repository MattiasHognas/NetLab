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
    using System.Threading;
    using System.Threading.Tasks;
    using Boxed.AspNetCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.WebUtilities;
    using Moq;
    using Workspace.Service.ViewModels;
    using Xunit;
    using Xunit.Abstractions;

    public class BookControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public BookControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Delete_BookFound_Returns204NoBookAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var book = new List<Models.Book> { new Models.Book() };
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            this.BookRepositoryMock.Setup(x => x.DeleteAsync(book.First(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var response = await this.client.DeleteAsync(new Uri("/book/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_BookNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 999,
            };
            var book = new List<Models.Book>();
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var response = await this.client.DeleteAsync(new Uri("/book/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_BookFound_Returns200OkAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var book = new List<Models.Book> { new Models.Book { BookId = 1 } };
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var bookViewModel = await response.Content.ReadAsAsync<List<Models.Book>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(book.First().BookId, bookViewModel.First().BookId);
        }

        [Fact]
        public async Task Get_BookNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 999,
            };
            var book = new List<Models.Book>();
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406MethodNotAllowedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/book/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_BookByBookFound_Returns200OkAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var book = new List<Models.Book> { new Models.Book { BookId = 1, ModifiedDate = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) } };
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var bookViewModel = await response.Content.ReadAsAsync<List<Book>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(book.First().BookId, bookViewModel.First().BookId);
        }

        [Fact]
        public async Task Get_BookByBookNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 999,
            };
            var book = new List<Models.Book>();
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeaderByBook_Returns406NotAcceptableAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var book = new List<Models.Book>();
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_BookByBookHasBeenModifiedSince_Returns200OKAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var book = new List<Models.Book> { new Models.Book() { ModifiedDate = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) } };
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);
            var uriString = AddQueryString("/book", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostBook_Valid_Returns201CreatedAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var saveBook = new SaveBook()
            {
                Name = "x",
                Description = "x",
            };
            var book = new Models.Book() { BookId = 1 };
            this.BookRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Book>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);

            var response = await this.client.PostAsJsonAsync("book", saveBook).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var bookViewModel = await response.Content.ReadAsAsync<Book>(this.formatters).ConfigureAwait(false);
            var uriString = AddQueryString("/book", filters);
            Assert.Equal(new Uri($"http://localhost{uriString}"), response.Headers.Location);
        }

        [Fact]
        public async Task PostBook_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("book", new SaveBook()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostBook_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "book")
            {
                Content = new ObjectContent<SaveBook>(null!, new JsonMediaTypeFormatter(), ContentType.Json),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostBook_UnsupportedMediaType_Returns415UnsupportedMediaTypeAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "book")
            {
                Content = new ObjectContent<SaveBook>(new SaveBook(), new JsonMediaTypeFormatter(), ContentType.Text),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, problemDetails.Status);
        }

        [Fact]
        public async Task PutBook_Valid_Returns200OkAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 1,
            };
            var saveBook = new SaveBook()
            {
                Name = "x",
                Description = "x",
            };
            var book = new List<Models.Book> { new Models.Book() { BookId = 1 } };
            this.BookRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(book);
            this.BookRepositoryMock.Setup(x => x.UpdateAsync(book.First(), It.IsAny<CancellationToken>())).ReturnsAsync(book.First());

            var response = await this.client.PutAsJsonAsync("book/1", saveBook).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var bookViewModel = await response.Content.ReadAsAsync<Book>(this.formatters).ConfigureAwait(false);
            Assert.Equal(bookViewModel.BookId, book.First().BookId);
        }

        [Fact]
        public async Task PutBook_BookNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.BookOptionFilter
            {
                BookId = 999,
            };
            var saveBook = new SaveBook()
            {
                Name = "x",
                Description = "x",
            };
            var book = new List<Models.Book>();
            this.BookRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.BookOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(book);

            var response = await this.client.PutAsJsonAsync("book/999", saveBook).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutBook_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("book/1", new SaveBook()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        private static string AddQueryString(string uriString, Models.BookOptionFilter filters)
        {
            var provider = CultureInfo.InvariantCulture;

            if (filters.BookId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"BookId", filters.BookId.Value.ToString(provider));
            }

            return uriString;
        }
    }
}
