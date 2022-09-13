using Blog.Application.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Security.Claims;

namespace Blog.IntegrationTests.MVC;

public class MVCIntegrationTests : IClassFixture<TestWebAppFactory<Program>>
{
    private readonly TestWebAppFactory<Program> _factory;
    private readonly HttpClient _anonymousClient;
    private readonly HttpClient _adminClient;
    private readonly HttpClient _normalClient;
    private readonly ITestOutputHelper _output;
    public MVCIntegrationTests(TestWebAppFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;


        _anonymousClient = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });

        _normalClient = _factory.CreateClientWithAuth(
            new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestNormalUser"),
            },
            new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false }
            );

        _adminClient = _factory.CreateClientWithAuth(
            new Claim[]
            {
                new Claim(ClaimTypes.Name, "TestAdminUser"),
                new Claim(ClaimTypes.Role, RolesConstants.ADMIN_ROLE)
            },
            new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false }
            );

        _output = output;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/all")]
    [InlineData("/all?tagname=one")]
    [InlineData("/account/signup")]
    [InlineData("/account/login")]
    [InlineData("/post/1")]
    public async Task GetPublicRoutes_HasSuccessStatusCode(string url)
    {
        var response = await _anonymousClient.GetAsync(url);
        _output.WriteLine(response.StatusCode.ToString());
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task GetDraftPostAsAnonymous_ReturnsNotFound()
    {
        var response = await _anonymousClient.GetAsync("/post/3");
        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
    }

    [Fact]
    public async Task GetPost_ReturnsPostPage()
    {
        var response = await _anonymousClient.GetAsync("/post/1");
        var content = await response.Content.ReadAsStringAsync();
        Assert.True(response.IsSuccessStatusCode);
        Assert.Contains("Post 1", content);
        Assert.Contains("one", content);
        Assert.Contains("two", content);
        Assert.Contains("three", content);
    }

    [Theory]
    [InlineData("/post/create")]
    [InlineData("/post/update/1")]
    [InlineData("/manage/userlist")]
    public async Task GetAdminPageAdAdmin_ReturnsPage(string path)
    {
        var response = await _adminClient.GetAsync(path);
        _output.WriteLine(await response.Content.ReadAsStringAsync());
        _output.WriteLine(response.StatusCode.ToString());
        Assert.True(response.IsSuccessStatusCode);
    }

}