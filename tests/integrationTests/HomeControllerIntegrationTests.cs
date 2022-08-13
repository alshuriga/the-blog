using Microsoft.AspNetCore.Authentication;

public class HomeControllerIntegrationTests : IntegrationTestsBase
{
    public HomeControllerIntegrationTests(CustomWebAppFactory<Program> factory) : base(factory) {}
        
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
        Assert.Equal(System.Net.HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task PostTags_ReturnsNewPostPage()
    {
        var postForm = new FormUrlEncodedContent(new Dictionary<string, string> {
           { "username", "admin"}, { "password", "admin" }
         }); 
        var result = await _client.GetAsync("/admin/userlist");
        Assert.Equal("", result.Headers.Location?.AbsoluteUri);
    }

}