// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.EntityFrameworkCore;
// using MiniBlog.Infrastructure.Data;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.DependencyInjection;
// using System.Net;
// using Microsoft.Net.Http.Headers;
// using MiniBlog.Tests.Config;

// namespace MiniBlog.Tests;

// public class HomeControllerIntegrationTests : IntegrationTestsBase
// {
//     private readonly ITestOutputHelper _testOutputHelper;
//     public HomeControllerIntegrationTests(CustomWebAppFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(factory) { 
//         _testOutputHelper = testOutputHelper;
//     }

//     [Fact]
//     public async Task GetToRoot_ReturnsIndexPage()
//     {
//         var result = await _client.GetAsync("/");
//         var contentTypeHeader = result.Content.Headers.FirstOrDefault(h => h.Key == "Content-Type").Value.FirstOrDefault();
//         Assert.True(result.IsSuccessStatusCode);
//         Assert.True(contentTypeHeader?.Contains("text/html"));
//     }

//     [Fact]
//     public async Task GetToNonExistingUrl_Returns404()
//     {
//         var result = await _client.GetAsync("/nonexistingaddress");
//         Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
//     }


//     [Fact]
//     public async Task CreatePost_AsAdmin_ReturnsPostPage()
//     {
//         //GET request for antiforgery data  
//         var antiforgeryRequest = new HttpRequestMessage(HttpMethod.Get, "/post/new");
//         antiforgeryRequest.AppendFakeAuth(isAdmin: true);
//         var antiforgeryData = await (await _client.SendAsync(antiforgeryRequest)).ExtractAntiforgeryKeys();

//         //generating unique guid header
//         var testHeader = Guid.NewGuid().ToString();
//         var postForm = new FormUrlEncodedContent(new Dictionary<string, string> {
//             { "Post.PostId", "0" },
//             { "Post.DateTime", DateTime.Now.ToString() },
//             { "Post.Text", "Test post. Test post. Test post. Test post. "},
//             { "Post.Header", testHeader },
//             { "TagString", "one,two,three" },
//             { AntiForgeryExtractor.AntiforgeryFormFieldName, antiforgeryData.field }
//          });

//         var postRequest = new HttpRequestMessage(HttpMethod.Post, "/post/save");
//         postRequest.AppendFakeAuth(isAdmin: true);
//         postRequest.Headers.Add("Cookies", new CookieHeaderValue(AntiForgeryExtractor.AntiforgeryCookieName, antiforgeryData.cookie).ToString());
//         postRequest.Content = postForm;
//         var result = await _client.SendAsync(postRequest);
//         var db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<MiniBlogEfContext>();
//         var newPostId = db.Posts.Where(p => p.Header == testHeader).First().PostId;
        
//         Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
//         Assert.Equal(1, db.Posts.Where(p => p.Header == testHeader && p.PostId == newPostId).Count());
//         Assert.Contains($"post/{newPostId}", result.Headers.Location?.OriginalString);
//     }
// }