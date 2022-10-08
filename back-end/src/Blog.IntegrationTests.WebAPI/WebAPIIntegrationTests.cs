using Blog.Application.Features.Posts.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace Blog.IntegrationTests.WebAPI;

public class WebApiIntegrationTests : IClassFixture<TestWebAppFactory<Program>>
{
    private readonly TestWebAppFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    public WebApiIntegrationTests(TestWebAppFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _output = output;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions() { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task GetDraftPostAsAnonymous_ReturnsNotFound()
    {
        var response = await _client.GetAsync("api/post/singlepost/1");
        var postVM = JsonConvert.DeserializeObject<PostSingleVM>(await response.Content.ReadAsStringAsync());

        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal(1, postVM?.Post.Id);
        Assert.Equal("Post 1", postVM?.Post.Header);
    }

}