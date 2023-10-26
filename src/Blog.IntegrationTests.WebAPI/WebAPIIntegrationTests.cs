using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.ViewModels;
using Blog.Application.Features.User.DTO;
using Blog.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;

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
    public async Task GetNoDraftPost_ReturnCorrectPost()
    {
        var response = await _client.GetAsync("api/post/1");
        var postVM = JsonConvert.DeserializeObject<PostSingleVM>(await response.Content.ReadAsStringAsync());

        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal(1, postVM?.Post.Id);
        Assert.Equal("Post 1", postVM?.Post.Header);
    }

    [Fact]
    public async Task GetDraftPostAsAnonymous_ReturnNotFoundStatus()
    {
        var response = await _client.GetAsync("api/post/3");
        var postVM = JsonConvert.DeserializeObject<PostSingleVM>(await response.Content.ReadAsStringAsync());

        Assert.Equal(StatusCodes.Status404NotFound, (int)response.StatusCode);
        Assert.Null(postVM!.Post);
        Assert.Null(postVM!.Commentaries);
    }

    [Fact]
    public async Task UpdatePostAsAdmin_ReturnNoContentAndPostUpdatesSuccessfully()
    {
        //arrange
        var jwt = await GetJwt(isAdmin: true);

        var post = new UpdatePostDTO()
        {
            Id = 2,
            TagString = "updating, post, test",
            Text = "This is the post updating test.",
            Header = "Post updating test"
        };

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri("http://localhost/api/post"),
            Content = JsonContent.Create(post),
            Method = HttpMethod.Put
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwt);

        //act
        var response = await _client.SendAsync(request);
        var updatedPostResponse = await _client.GetAsync("api/post/2");
        var updatedPost = (await updatedPostResponse.Content.ReadFromJsonAsync<PostSingleVM>())?.Post;


        //assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        Assert.Equal(StatusCodes.Status200OK, (int)updatedPostResponse.StatusCode);
        Assert.Equal(post.Id, updatedPost?.Id);
        Assert.Equal(post.Text, updatedPost?.Text);
        Assert.Equal(post.Header, updatedPost?.Header);
        Assert.Equal(post.TagString.Split(',').OrderBy(t => t).Select(t => t.Trim().ToLower()),
            updatedPost?.Tags.OrderBy(t => t.Name).Select(t => t.Name.Trim().ToLower()));
    }

    [Fact]
    public async Task DeletePostAsAdmin_ReturnsNoContentAndNotFoundForPost()
    {
        var jwt = await GetJwt(isAdmin: true);

        var postIdForDeletion = 3;

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri($"http://localhost/api/post/{postIdForDeletion}"),
            Method = HttpMethod.Delete
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwt);

        //act
        var response = await _client.SendAsync(request);
        var deletedPostResponse = await _client.GetAsync($"api/post/{postIdForDeletion}");


        //assert
        Assert.Equal(StatusCodes.Status204NoContent, (int)response.StatusCode);
        Assert.Equal(StatusCodes.Status404NotFound, (int)deletedPostResponse.StatusCode);
    }


    [Fact]
    public async Task CreatePostAsAdmin_ReturnNoContentAndPostCreatesSuccessfully()
    {
        var jwt = await GetJwt(isAdmin: true);

        var post = new UpdatePostDTO()
        {
            TagString = "updating, post, test",
            Text = "This is the post updating test.",
            Header = "Post updating test"
        };

        var request = new HttpRequestMessage()
        {
            RequestUri = new Uri("http://localhost/api/post"),
            Content = JsonContent.Create(post),
            Method = HttpMethod.Post
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", jwt);

        //act
        var response = await _client.SendAsync(request);
        var newPostId = long.Parse(await response.Content.ReadAsStringAsync());
        var createdPostResponse = await _client.GetAsync($"api/post/{newPostId}");
        var createdPost = (await createdPostResponse.Content.ReadFromJsonAsync<PostSingleVM>())?.Post;


        //assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);
        Assert.Equal(StatusCodes.Status200OK, (int)createdPostResponse.StatusCode);
        Assert.Equal(newPostId, createdPost?.Id);
        Assert.Equal(post.Text, createdPost?.Text);
        Assert.Equal(post.Header, createdPost?.Header);
        Assert.Equal(post.TagString.Split(',').OrderBy(t => t).Select(t => t.Trim().ToLower()),
            createdPost?.Tags.OrderBy(t => t.Name).Select(t => t.Name.Trim().ToLower()));
    }






    private async Task<string> GetJwt(bool isAdmin)
    {
        var creds = isAdmin ?
            new UserSignInDTO() { Username = "admin", Password = "admin" } :
            new UserSignInDTO() { Username = "normal", Password = "12345" };

        var jwtResponse = await _client.PostAsJsonAsync("api/account/login", creds);
        var jwt = (await jwtResponse.Content.ReadFromJsonAsync<JwtToken>())?.Token;

        if (jwt == null) 
            throw new Exception("Error getting jwt");

        return jwt;
    }

}

