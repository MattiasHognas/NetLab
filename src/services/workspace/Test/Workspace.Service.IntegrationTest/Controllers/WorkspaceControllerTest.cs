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

    public class WorkspaceControllerTest : CustomWebApplicationFactory<Startup>
    {
        private readonly HttpClient client;
        private readonly MediaTypeFormatterCollection formatters;

        public WorkspaceControllerTest(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            this.client = this.CreateClient();
            this.formatters = new MediaTypeFormatterCollection();
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.RestfulJson));
            this.formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(ContentType.ProblemJson));
        }

        [Fact]
        public async Task Options_WorkspaceRoot_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "workspace/1");

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
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var uriString = AddQueryString("workspace", filters);
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
        public async Task Options_WorkspaceId_Returns200OkAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Options, "workspace/1");

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
        public async Task Delete_WorkspaceFound_Returns204NoWorkspaceAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace> { new Models.Workspace() };
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            this.WorkspaceRepositoryMock.Setup(x => x.DeleteAsync(workspace.First(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var response = await this.client.DeleteAsync(new Uri("/workspace/1", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_WorkspaceNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 999,
            };
            var workspace = new List<Models.Workspace>();
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var response = await this.client.DeleteAsync(new Uri("/workspace/999", UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_WorkspaceFound_Returns200OkAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace> { new Models.Workspace { WorkspaceId = 1 } };
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var workspaceViewModel = await response.Content.ReadAsAsync<List<Models.Workspace>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(workspace.First().WorkspaceId, workspaceViewModel.First().WorkspaceId);
        }

        [Fact]
        public async Task Get_WorkspaceNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 999,
            };
            var workspace = new List<Models.Workspace>();
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeader_Returns406MethodNotAllowedAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/workspace/1");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.MethodNotAllowed, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_WorkspaceByWorkspaceFound_Returns200OkAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace> { new Models.Workspace { WorkspaceId = 1, Modified = new DateTimeOffset(2000, 1, 2, 3, 4, 5, TimeSpan.FromHours(6)) } };
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var workspaceViewModel = await response.Content.ReadAsAsync<List<Workspace>>(this.formatters).ConfigureAwait(false);
            Assert.Equal(workspace.First().WorkspaceId, workspaceViewModel.First().WorkspaceId);
        }

        [Fact]
        public async Task Get_WorkspaceByWorkspaceNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 999,
            };
            var workspace = new List<Models.Workspace>();
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            var response = await this.client.GetAsync(new Uri(uriString, UriKind.Relative)).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task Get_InvalidAcceptHeaderByWorkspace_Returns406NotAcceptableAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace>();
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(ContentType.Text));

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotAcceptable, response.StatusCode);
            // Note: ASP.NET Core should be automatically returning a ProblemDetails response but is returning an empty
            // response body instead. See https://github.com/aspnet/AspNetCore/issues/16889
        }

        [Fact]
        public async Task Get_WorkspaceByWorkspaceHasBeenModifiedSince_Returns200OKAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace> { new Models.Workspace() { Modified = new DateTimeOffset(2000, 1, 1, 0, 0, 1, TimeSpan.Zero) } };
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            var uriString = AddQueryString("/workspace", filters);
            using var request = new HttpRequestMessage(HttpMethod.Get, uriString);
            request.Headers.IfModifiedSince = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PostWorkspace_Valid_Returns201CreatedAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var saveWorkspace = new SaveWorkspace()
            {
                WorkspaceId = 1,
                Name = "x",
                Description = "x",
            };
            var workspace = new Models.Workspace() { WorkspaceId = 1 };
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.WorkspaceRepositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Models.Workspace>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(workspace);

            var response = await this.client.PostAsJsonAsync("workspace", saveWorkspace).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var workspaceViewModel = await response.Content.ReadAsAsync<Workspace>(this.formatters).ConfigureAwait(false);
            var uriString = AddQueryString("/workspace", filters);
            Assert.Equal(new Uri($"http://localhost{uriString}"), response.Headers.Location);
        }

        [Fact]
        public async Task PostWorkspace_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PostAsJsonAsync("workspace", new SaveWorkspace()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostWorkspace_EmptyRequestBody_Returns400BadRequestAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "workspace")
            {
                Content = new ObjectContent<SaveWorkspace>(null!, new JsonMediaTypeFormatter(), ContentType.Json),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PostWorkspace_UnsupportedMediaType_Returns415UnsupportedMediaTypeAsync()
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "workspace")
            {
                Content = new ObjectContent<SaveWorkspace>(new SaveWorkspace(), new JsonMediaTypeFormatter(), ContentType.Text),
            };

            var response = await this.client.SendAsync(request).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status415UnsupportedMediaType, problemDetails.Status);
        }

        [Fact]
        public async Task PutWorkspace_Valid_Returns200OkAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var saveWorkspace = new SaveWorkspace()
            {
                WorkspaceId = 1,
                Name = "x",
                Description = "x",
            };
            var workspace = new List<Models.Workspace> { new Models.Workspace() { WorkspaceId = 1 } };
            this.WorkspaceRepositoryMock
                .Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(workspace);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.WorkspaceRepositoryMock.Setup(x => x.UpdateAsync(workspace.First(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace.First());

            var response = await this.client.PutAsJsonAsync("workspace/1", saveWorkspace).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ContentType.RestfulJson, response.Content.Headers.ContentType?.MediaType);
            var workspaceViewModel = await response.Content.ReadAsAsync<Workspace>(this.formatters).ConfigureAwait(false);
            Assert.Equal(workspaceViewModel.WorkspaceId, workspace.First().WorkspaceId);
        }

        [Fact]
        public async Task PutWorkspace_WorkspaceNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 999,
            };
            var saveWorkspace = new SaveWorkspace()
            {
                WorkspaceId = 1,
                Name = "x",
                Description = "x",
            };
            var workspace = new List<Models.Workspace>();
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);

            var response = await this.client.PutAsJsonAsync("workspace/999", saveWorkspace).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PutWorkspace_Invalid_Returns400BadRequestAsync()
        {
            var response = await this.client.PutAsJsonAsync("workspace/1", new SaveWorkspace()).ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        }

        [Fact]
        public async Task PatchWorkspace_WorkspaceNotFound_Returns404NotFoundAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 999,
            };
            var workspace = new List<Models.Workspace>();
            var patch = new JsonPatchDocument<SaveWorkspace>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);

            var response = await this.client
                .PatchAsync(new Uri("workspace/999", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(this.formatters).ConfigureAwait(false);
            Assert.Equal(StatusCodes.Status404NotFound, problemDetails.Status);
        }

        [Fact]
        public async Task PatchWorkspace_InvalidWorkspace_Returns400BadRequestAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var workspace = new List<Models.Workspace>
            {
                new Models.Workspace { WorkspaceId = 1 },
            };
            var patch = new JsonPatchDocument<SaveWorkspace>();
            patch.Remove(x => x.Name);
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);

            var response = await this.client
                .PatchAsync(new Uri("workspace/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchWorkspace_ValidWorkspace_Returns200OkAsync()
        {
            var filters = new Models.WorkspaceOptionFilter
            {
                WorkspaceId = 1,
            };
            var patch = new JsonPatchDocument<SaveWorkspace>();
            patch.Add(x => x.Name, "x");
            var json = JsonConvert.SerializeObject(patch);
            using var strcontent = new StringContent(json, Encoding.UTF8, ContentType.JsonPatch);
            var workspace = new List<Models.Workspace> { new Models.Workspace() { WorkspaceId = 1, Name = "x", Description = "x" } };
            this.WorkspaceRepositoryMock.Setup(x => x.GetAsync(It.IsAny<Models.WorkspaceOptionFilter>(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace);
            this.ClockServiceMock.SetupGet(x => x.UtcNow).Returns(new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero));
            this.WorkspaceRepositoryMock.Setup(x => x.UpdateAsync(workspace.First(), It.IsAny<CancellationToken>())).ReturnsAsync(workspace.First());

            var response = await this.client
                .PatchAsync(new Uri("workspace/1", UriKind.Relative), strcontent)
                .ConfigureAwait(false);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var workspaceViewModel = await response.Content.ReadAsAsync<Workspace>(this.formatters).ConfigureAwait(false);
            Assert.Equal("x", workspaceViewModel.Name);
        }

        private static string AddQueryString(string uriString, Models.WorkspaceOptionFilter filters)
        {
            var provider = CultureInfo.InvariantCulture;

            if (filters.WorkspaceId.HasValue)
            {
                uriString = QueryHelpers.AddQueryString(uriString, $"WorkspaceId", filters.WorkspaceId.Value.ToString(provider));
            }

            return uriString;
        }
    }
}
