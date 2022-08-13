using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace MiniBlog.Tests;

public class HomeControllerIntegrationTests : IntegrationTestsBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    public HomeControllerIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory) { 
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task GetToRoot_ReturnsIndexPage()
    {
        var result = await _client.GetAsync("/");
        var contentTypeHeader = result.Content.Headers.FirstOrDefault(h => h.Key == "Content-Type").Value.FirstOrDefault();
        Assert.True(result.IsSuccessStatusCode);
        Assert.True(contentTypeHeader?.Contains("text/html"));
    }

    [Fact]
    public async Task GetToNonExistingUrl_Returns404()
    {
        var result = await _client.GetAsync("/nonexistingaddress");
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task AdminUserList_AuthAsAdmin_ReturnsUserListPageOK()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/userlist");
        request.AppendFakeAuth(isAdmin: true);

        var result = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        Assert.Contains("Users List", (await result.Content.ReadAsStringAsync()));
    }


    [Fact]
    public async Task AdminUserList_AuthAsNonAdmin_ReturnsAccessDenied302()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/userlist");
        request.AppendFakeAuth(isAdmin: false);

        var result = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        Assert.Contains("AccessDenied", result.Headers.Location?.AbsolutePath);
    }

    [Fact]
    public async Task AdminUserList_AsAnonymous_ReturnsLogin302()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/admin/userlist");

        var result = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        Assert.Contains("Login", result.Headers.Location?.AbsolutePath);
    }


    [Fact]
    public async Task CreatePost_AsAdmin_ReturnsPostPage()
    {
        //TODO: Handle antiforgery token 
        var testHeader = Guid.NewGuid().ToString();
        var postForm = new FormUrlEncodedContent(new Dictionary<string, string> {
            { "Post.PostId", "0" },
            { "Post.DateTime", DateTime.Now.ToString() },
           { "Post.Text", "Test post. Test post. Test post. Test post. "},
           { "Post.Header", testHeader },
           { "TagString", "one,two,three" }
         });

        var request = new HttpRequestMessage(HttpMethod.Post, "/post/save");
        request.AppendFakeAuth(isAdmin: true);
        request.Content = postForm;
        var result = await _client.SendAsync(request);
        var db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<MiniBlogEfContext>();
        var newPostId = db.Posts.Where(p => p.Header == testHeader).First().PostId;
        Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        Assert.Equal(1, db.Posts.Where(p => p.Header == testHeader).Count());
        Assert.Contains($"post/{newPostId}", result.Headers.Location?.OriginalString);
    }
}