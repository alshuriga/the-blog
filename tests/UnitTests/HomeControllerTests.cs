using MiniBlog.Web.Controllers;
using MiniBlog.Web.ViewModels;
using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Core.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MiniBlog.Tests;

public class HomeControllersTests
{
    private Mock<IPostsRepo> PostsRepoMock { get; set; } = new();
    private Mock<UserManager<IdentityUser>> UserManagerMock { get; set; } = new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
    private Mock<ILogger<HomeController>> LoggerMock { get; set; } = new();
    private HomeController GetTestControllerWithMocks() => new HomeController(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);


    [Fact]
    public async void Index_SecondPageOfPosts_ReturnsViewWithCorrectPostsModel()
    {
        var repoAllPosts = SeedPosts(10).ToList();
        IEnumerable<Post> pageOfPosts = repoAllPosts.Skip(5).Take(5).ToList();
        PaginateParams paginateParams = new(5, 5);
        PostsRepoMock.Setup(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), null)).ReturnsAsync(pageOfPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count);

        var controller = GetTestControllerWithMocks();
        var resultModel = (await controller.Index(currentPage: 2) as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.RetrievePostsRange(It.Is<PaginateParams>(p => p.Skip == 5 && p.Take == 5), null),
            Times.Once());
        PostsRepoMock.Verify(r => r.GetPostsCount(It.Is<string?>(s => s == null)), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Equal(pageOfPosts, resultModel?.Posts);
        Assert.Equal(2, resultModel?.PaginationData?.PageNumber);
    }

    [Fact]
    public async void Index_GetLessPostsThanPerPage_ReturnsViewWithNullPagination()
    {
        IEnumerable<Post> pageOfPosts = SeedPosts(3);

        PostsRepoMock.Setup(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), null)).ReturnsAsync(pageOfPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(3);

        var controller = GetTestControllerWithMocks();
        var resultModel = (await controller.Index(currentPage: 2) as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.RetrievePostsRange(It.Is<PaginateParams>(p => p.Skip == 0 && p.Take == 5), null),
            Times.Once());
        PostsRepoMock.Verify(r => r.GetPostsCount(null), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Null(resultModel?.PaginationData);
    }

    [Fact]
    public async void Index_RepoHasMultiplePosts_ReturnsCorrectPagesNumber()
    {
        var repoAllPosts = SeedPosts(24).ToList();
        var pageOfPosts = repoAllPosts.Skip(10).Take(5).ToList();
        int expectedPageNumber = 5;
        PostsRepoMock.Setup(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), null)).ReturnsAsync(pageOfPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count);

        var controller = GetTestControllerWithMocks();
        var resultModel = (await controller.Index(3) as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.RetrievePostsRange(It.Is<PaginateParams>(p => p.Skip == 10 && p.Take == 5), null),
            Times.Once());
        PostsRepoMock.Verify(r => r.GetPostsCount(null), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Equal(expectedPageNumber, resultModel?.PaginationData?.PageNumber);
    }

    [Fact]
    public async void Index_GetPostsByTag_ReturnsCorrectPosts()
    {
        var repoAllPosts = SeedPosts(24).ToList();
        var repoPostsWithCorrectTag = repoAllPosts.Take(10).ToList();
        Tag testTag_1 = new() { TagId = 1, Name = "firsttag" };
        Tag testTag_2 = new() { TagId = 2, Name = "secondtag" };
        for (int i = 0; i < 10; i++)
        {
            repoAllPosts.ElementAt(i).Tags = new List<Tag>() { testTag_1, testTag_2 };
        }

        PostsRepoMock.Setup(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), It.IsAny<string>()))
            .ReturnsAsync(repoPostsWithCorrectTag);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.Is<string>(s => s == "firsttag"))).ReturnsAsync(repoAllPosts.Count());

        var controller = GetTestControllerWithMocks();
        var resultViewModel =
            (await controller.Index(tagName: "firsttag") as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), It.Is<string>(s => s == "firsttag")),
            Times.Once);
        PostsRepoMock.Verify(r => r.GetPostsCount(It.Is<String>(s => s == "firsttag")), Times.Once);
        PostsRepoMock.Verify(r => r.GetPostsCount(null), Times.Never);
        PostsRepoMock.Verify(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), null), Times.Never);
        Assert.Equal(10, resultViewModel?.Posts.Count());
    }

    [Fact]
    public async void Post_GetPostById_ReturnsViewWithCorrectPost()
    {
        Post post = SeedPosts(1).First();
        for (int i = 0; i < 5; i++)
            post.Commentaries.Add(new Commentary()
            { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });

        PostsRepoMock.Setup(r => r.GetCommentariesCount(It.IsAny<long>())).ReturnsAsync(5);
        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>())).ReturnsAsync(post);


        var controller = GetTestControllerWithMocks();
        controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        var resultModel = (await controller.Post(postId: 1) as ViewResult)?.Model as SinglePostPageViewModel;

        PostsRepoMock.Verify(r => r.GetCommentariesCount(1), Times.Once);
        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
        Assert.True(controller.TempData.ContainsKey("postId"));
        Assert.Equal("1", controller.TempData["postId"]);
        Assert.NotNull(resultModel);
        Assert.Equal(post, resultModel?.Post);
    }

    [Fact]
    public async void Post_GetPostByInvalidId_ReturnsNotFound()
    {
        Post post = SeedPosts(1).First();

        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()))
            .ReturnsAsync((Post?)null);

        var controller = GetTestControllerWithMocks();
        var result = await controller.Post(5);
        PostsRepoMock.Verify(r => r.GetCommentariesCount(5), Times.Once);
        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async void CreateOrUpdatePost_AddPost_ReturnsRedirectToPost()
    {
        Post postForModel = SeedPosts(1).First();
        postForModel.Tags = new List<Tag>();
        postForModel.PostId = 0; //new post, does not exist in database
        for (int i = 0; i < 5; i++)
            postForModel.Commentaries.Add(new Commentary()
            { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3" };

        PostsRepoMock.Setup(r => r.CreateTagIfNotExist(It.IsAny<Tag>())).Callback((Tag t) => postForModel.Tags.Add(t));
        PostsRepoMock.Setup(r => r.CreatePost(It.IsAny<Post>())).Callback(() => postForModel.PostId = 1)
            .ReturnsAsync(1);


        var controller = GetTestControllerWithMocks();
        var result = await controller.CreateOrUpdatePost(postEditViewModel);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(3, postEditViewModel.Post.Tags.Count);
        Assert.Equal(1, postEditViewModel.Post.PostId);
        Assert.Equal(postEditViewModel.Post.PostId, ((RedirectToActionResult)result).RouteValues?["postid"]);
        Assert.True(Enumerable.SequenceEqual(new string[] { "tag1", "tag2", "tag3" },
            postEditViewModel.Post.Tags.Select(t => t.Name).ToArray()));
        PostsRepoMock.Verify(r => r.CreatePost(postForModel), Times.Once);
        PostsRepoMock.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async void CreateOrUpdatePost_UpdatePost_ReturnsRedirectToPost()
    {
        Post postForModel = new Post()
        {
            PostId = 7,
            Header = "Test",
            Text = "test",
            DateTime = DateTime.Now
        }; //this post exists in the database, id is 7
        for (int i = 0; i < 5; i++)
        {
            postForModel.Commentaries.Add(new Commentary()
            { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });
        }

        PostsRepoMock.Setup(r => r.CreateTagIfNotExist(It.IsAny<Tag>())).Callback((Tag t) => postForModel.Tags.Add(t));

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3" };

        var controller = GetTestControllerWithMocks();
        var result = await controller.CreateOrUpdatePost(postEditViewModel);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(7, postEditViewModel.Post.PostId);
        Assert.Equal(postEditViewModel.Post.PostId, ((RedirectToActionResult)result).RouteValues?["postid"]);
        Assert.True(Enumerable.SequenceEqual(new string[] { "tag1", "tag2", "tag3" },
            postEditViewModel.Post.Tags.Select(t => t.Name).ToArray()));
        PostsRepoMock.Verify(r => r.CreateTagIfNotExist(It.IsAny<Tag>()), Times.Exactly(3));
        PostsRepoMock.Verify(r => r.CreatePost(postForModel), Times.Never);
        PostsRepoMock.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Once);
    }


    [Fact]
    public async void EditPost_PassExistingPostId_ReturnsViewForUpdating()
    {
        Post post = new Post() { PostId = 1, Header = "Test", Text = "test", DateTime = DateTime.Now };
        post.Tags = new List<Tag>()
        {
            new Tag() { TagId = 1, Name = "test1" },
            new Tag() { TagId = 2, Name = "test2" },
            new Tag() { TagId = 3, Name = "test3" }
        };
        string tagString_expected = "test1,test2,test3";
        PostsRepoMock.Setup(r => r.RetrievePost(1, It.IsAny<PaginateParams>())).ReturnsAsync(post);

        var controller = GetTestControllerWithMocks();
        var result = await controller.EditPost(1);
        var resultModel = (result as ViewResult)?.Model as PostEditViewModel;

        Assert.IsType<ViewResult>(result);
        Assert.NotNull(resultModel);
        Assert.Equal(tagString_expected, resultModel?.TagString);
        Assert.Equal(3, resultModel?.Post.Tags.Count);
        Assert.Equal(1, resultModel?.Post.PostId);
        Assert.Equal("Edit Post", (result as ViewResult)?.ViewData["title"]);
    }


    [Fact]
    public async void EditPost_PassNullPostId_ReturnsViewForCreating()
    {
        PostsRepoMock.Setup(r => r.RetrievePost(1, It.IsAny<PaginateParams>())).ReturnsAsync((Post?)null);
        var controller = GetTestControllerWithMocks();
        var result = await controller.EditPost(null);
        var resultModel = (result as ViewResult)?.Model as PostEditViewModel;
        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Never);
        Assert.IsType<ViewResult>(result);
        Assert.NotNull(resultModel);
        Assert.Equal(0, resultModel?.Post.PostId);
        Assert.Equal("New Post", (result as ViewResult)?.ViewData["title"]);
    }

    [Fact]
    public async void EditPost_PassNonexistingPostId_ReturnsNotFound()
    {
        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()))
            .ReturnsAsync((Post?)null);

        var controller = GetTestControllerWithMocks();
        var result = await controller.EditPost(245); //non-existing post id

        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void DeletePost_PassExistingPostId_ReturnsRedirectToIndex()
    {
        long postId = 15;
        var controller = GetTestControllerWithMocks();

        var result = await controller.DeletePost(postId);

        PostsRepoMock.Verify(r => r.DeletePost(It.Is<long>(l => l == postId)), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void DeletePost_PassZeroAsPostId_ReturnsNotFound()
    {
        long postId = 0;
        var controller = GetTestControllerWithMocks();

        var result = await controller.DeletePost(postId);

        PostsRepoMock.Verify(r => r.DeletePost(It.Is<long>(l => l == postId)), Times.Never);
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async void AddComment_AuthenticatedUser_AddsCommentAndRedirectsToPost()
    {
        // Given
        var userName = "TestUserName";
        var postId = 10;
        var email = "test-email@test.com";
        var controller = GetTestControllerWithMocks();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
            new Claim (ClaimTypes.NameIdentifier, "NameIdentifier"),
            new Claim(ClaimTypes.Name, userName)
        }, "TestIdentity"));
        var testUser = new IdentityUser{ UserName = userName, Email = email};
        UserManagerMock.Setup(m => m.FindByNameAsync(It.Is<string>(s => s == userName))).ReturnsAsync(testUser);
        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user};

        var testCommentary = new CommentaryViewModel { Text = "This is a test commentary" };
        // When
        var result = await controller.AddComment(testCommentary, postId);
        // Then    
        UserManagerMock.Verify(m => m.FindByNameAsync(It.Is<string>(s => s == userName)), Times.Once);
        PostsRepoMock.Verify(r => r.CreateComment(
            It.Is<Commentary>(c => c.Text == testCommentary.Text 
            && c.Username == testUser.UserName 
            && c.Email == testUser.Email), 
            It.Is<long>(l => l == postId)), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(postId, (long?)((RedirectToActionResult)result).RouteValues?["postId"]);
    }

    [Fact]
    public async void AddComment_AnonymousUser_RedirectsToLogin()
    {
        var anonUser = new ClaimsPrincipal(new ClaimsIdentity());
        var postId = 10;
        var testCommentary = new CommentaryViewModel { Text = "This is a test commentary" };
        var controller = GetTestControllerWithMocks();
        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = anonUser };

        var result = await controller.AddComment(testCommentary, postId);
 
        UserManagerMock.Verify(m => m.FindByNameAsync(It.IsAny<string>()), Times.Never);
        PostsRepoMock.Verify(r => r.CreateComment(It.IsAny<Commentary>(), It.IsAny<long>()), Times.Never);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Login", ((RedirectToActionResult)result).ActionName);
    }

    [Fact]
    public async void DeleteComment_PassExistingCommentId_RedirectsToPost()
    {
        var returnId = 1;
        var commId = 12;
        var controller = GetTestControllerWithMocks();

        var result = await controller.DeleteComment(commId, returnId);

        PostsRepoMock.Verify(r => r.DeleteComment(It.Is<long>(l => ((byte)l) == commId)), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(returnId, (long?)((RedirectToActionResult)result).RouteValues?["postId"]);
    }

    [Fact]
    public async void DeleteComment_PassExistingCommentIdAndNoPostId_RedirectsToPost()
    {
        var commId = 12;
        var controller = GetTestControllerWithMocks();

        var result = await controller.DeleteComment(commId, null);

        PostsRepoMock.Verify(r => r.DeleteComment(It.Is<long>(l => ((byte)l) == commId)), Times.Once);
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
    }

    //
    //private methods, not tests
    //
    private IEnumerable<Post> SeedPosts(int postNumber)
    {
        Post[] posts = new Post[postNumber];
        for (int i = 0; i < postNumber; i++)
        {
            Post post = new() { PostId = i + 1, Header = $"Post_{i + 1}", Text = new Guid().ToString() };
            posts[i] = post;
        }

        return posts;
    }

   
}