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

namespace MiniBlog.Tests;

public class HomeControllersTest
{
    private Mock<IPostsRepo> PostsRepoMock { get; set; } = new();
    private Mock<UserManager<IdentityUser>> UserManagerMock { get; set; } = new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null ,null, null, null, null ,null);
    private Mock<ILogger<HomeController>> LoggerMock { get; set; } = new();


    [Fact]
    public async void Index_SecondPageOfPosts_ReturnsViewWithCorrectPostsModel()
    {
        var repoAllPosts = SeedPosts(10).ToList();
        IEnumerable<Post> pageOfPosts = repoAllPosts.Skip(5).Take(5).ToList();
        PaginateParams paginateParams = new(5, 5);
        PostsRepoMock.Setup(r => r.RetrievePostsRange(It.IsAny<PaginateParams>(), null)).ReturnsAsync(pageOfPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        
        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
        controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        var resultModel = (await controller.Post(postId:1) as ViewResult)?.Model as SinglePostPageViewModel;

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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
        var result = await controller.Post(5);
        PostsRepoMock.Verify(r => r.GetCommentariesCount(5), Times.Once);
        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async void AddOrUpdatePost_AddPost_ReturnsRedirectToPost()
    {
        Post postForModel = SeedPosts(1).First();
        postForModel.Tags = new List<Tag>();
        postForModel.PostId = 0; //new post, does not exist in database
        for (int i = 0; i < 5; i++)
            postForModel.Commentaries.Add(new Commentary()
                { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3"};

        PostsRepoMock.Setup(r => r.CreateTagIfNotExist(It.IsAny<Tag>())).Callback((Tag t) => postForModel.Tags.Add(t));
        PostsRepoMock.Setup(r => r.CreatePost(It.IsAny<Post>())).Callback(() => postForModel.PostId = 1)
            .ReturnsAsync(1);

        
        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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
    public async void AddOrUpdatePost_UpdatePost_ReturnsRedirectToPost()
    {
        Post postForModel = new Post()
        {
            PostId = 7, Header = "Test", Text = "test", DateTime = DateTime.Now
        }; //this post exists in the database, id is 7
        for (int i = 0; i < 5; i++)
        {
            postForModel.Commentaries.Add(new Commentary()
                { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });
        }

        PostsRepoMock.Setup(r => r.CreateTagIfNotExist(It.IsAny<Tag>())).Callback((Tag t) => postForModel.Tags.Add(t));

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3" };

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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
        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
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

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, UserManagerMock.Object);
        var result = await controller.EditPost(245); //non-existing post id

        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
        Assert.IsType<NotFoundResult>(result);
    }
    

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