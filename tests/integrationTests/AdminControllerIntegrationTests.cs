using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.Net.Http.Headers;
using MiniBlog.Tests.Config;

namespace MiniBlog.Tests;

public class AdminControllerIntegrationTests : IntegrationTestsBase
{
    private readonly ITestOutputHelper _testOutputHelper;
    public AdminControllerIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory) { 
        _testOutputHelper = testOutputHelper;
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

}