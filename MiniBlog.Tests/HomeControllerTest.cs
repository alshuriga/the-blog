using MiniBlog.Controllers;
using MiniBlog.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MiniBlog.Tests;

public class HomeControllersTest
{

    Mock<IPostsRepo> PostsRepoMock = new();
    Mock<LinkGenerator> LinkGenMock = new();
    Mock<ILogger<HomeController>> LoggerMock = new();
    Mock<IHttpContextAccessor> AccessorMock = new();


    [Fact]
    public async void Index_SecondPageOfPosts_ReturnsViewWithCorrectPostsModel()
    {
        var repoAllPosts = SeedPosts(10);
        IEnumerable<Post> pageOfPosts = repoAllPosts.Skip(5).Take(5);

        PostsRepoMock.Setup(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(pageOfPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);
        var resultModel = (await controller.Index(currentPage: 2) as ViewResult)?.Model as MultiplePostsPageViewModel;


        Assert.NotNull(resultModel);
        Assert.Equal(pageOfPosts, resultModel?.Posts);
        Assert.Equal(2, resultModel?.PaginationData?.PageNumber);
    }

    [Fact]
    public async void Index_GetLessPostsThanPerPage_ReturnsViewWithNullPagination()
    {
        var repoAllPosts = SeedPosts(3);

        IEnumerable<Post> pageOfPosts = repoAllPosts;

        PostsRepoMock.Setup(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(repoAllPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var resultModel = (await controller.Index(currentPage: 2) as ViewResult)?.Model as MultiplePostsPageViewModel;


        PostsRepoMock.Verify(r => r.RetrieveMultiplePosts(0, 5, null), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Null(resultModel?.PaginationData);
    }

    [Fact]
    public async void Index_RepoHasMultiplePosts_ReturnsCorrectPagesNumber()
    {
        var repoAllPosts = SeedPosts(24);
        int expectedPageNumber = 5;
        PostsRepoMock.Setup(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(repoAllPosts);
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count());

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var resultModel = (await controller.Index() as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.GetPostsCount(null), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Equal(expectedPageNumber, resultModel?.PaginationData?.PageNumber);
    }

    [Fact]
    public async void Index_GetPostsByTag_ReturnsCorrectPosts()
    {
        var repoAllPosts = SeedPosts(24);
        var repoPostsWithCorrectTag = repoAllPosts.Take(10);
        Tag testTag_1 = new() { TagId = 1, Name = "firsttag" };
        Tag testTag_2 = new() { TagId = 2, Name = "secondtag" };
        for (int i = 0; i < 10; i++)
        {
            repoAllPosts.ElementAt(i).Tags = new List<Tag>() { testTag_1, testTag_2 };
        }
        PostsRepoMock.Setup(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), It.Is<string>(s => s == "firsttag"))).ReturnsAsync(repoPostsWithCorrectTag);
        PostsRepoMock.Setup(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), It.Is<string>(s => s == "noPostsWithThisTag"))).ReturnsAsync(Enumerable.Empty<Post>());
        PostsRepoMock.Setup(r => r.GetPostsCount(It.IsAny<string?>())).ReturnsAsync(repoAllPosts.Count());

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var resultModelTag1 = (await controller.Index(tagName: "firsttag") as ViewResult)?.Model as MultiplePostsPageViewModel;
        var resulModelNoPostsTag = (await controller.Index(tagName: "noPostsWithThisTag") as ViewResult)?.Model as MultiplePostsPageViewModel;

        PostsRepoMock.Verify(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), It.Is<string>(s => s == "firsttag")), Times.Once);
        PostsRepoMock.Verify(r => r.RetrieveMultiplePosts(It.IsAny<int>(),  It.IsAny<int>(), It.Is<string>(s => s == "noPostsWithThisTag")), Times.Once);
        PostsRepoMock.Verify(r => r.GetPostsCount(null), Times.Exactly(2));
        PostsRepoMock.Verify(r => r.RetrieveMultiplePosts(It.IsAny<int>(), It.IsAny<int>(), null), Times.Never);
        Assert.Equal(10, resultModelTag1?.Posts.Count());
        Assert.False(resulModelNoPostsTag?.Posts.Any());
    }

    [Fact]
    public async void Post_GetPostById_ReturnsViewWithCorrectPost()
    {
        Post post = SeedPosts(1).First();
        for(int i = 0; i < 5; i ++)
            post.Commentaries.Add( new Commentary() { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });

        PostsRepoMock.Setup(r => r.GetCommentsCount(It.IsAny<long>())).ReturnsAsync(5);
        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(post);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);
        
        var resultModel = (await controller.Post(1) as ViewResult)?.Model as SinglePostPageViewModel;
        
        PostsRepoMock.Verify(r => r.GetCommentsCount(1), Times.Once);
        PostsRepoMock.Verify(r => r.RetrievePost(1, 0, 5), Times.Once);
        Assert.NotNull(resultModel);
        Assert.Equal(post, resultModel?.Post);
        Assert.Equal(5, post.Commentaries.Count);
    }

    [Fact]
    public async void Post_GetPostByInvalidId_ReturnsNotFound()
    {
        Post post = SeedPosts(1).First();

        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Post?)null);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var result = await controller.Post(5);
        PostsRepoMock.Verify(r => r.GetCommentsCount(5), Times.Once);
        PostsRepoMock.Verify(r => r.RetrievePost(5, 0, 5), Times.Once);
        Assert.IsType<NotFoundResult>(result);
    }


    [Fact]
    public async void AddOrUpdatePost_AddPost_ReturnsRedirectToPost()
    {
        Post postForModel = SeedPosts(1).First();
        postForModel.PostId = 0; //new post, does not exist in database
        for(int i = 0; i < 5; i ++)
            postForModel.Commentaries.Add( new Commentary() { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3"};

        PostsRepoMock.Setup(r => r.CreateOrRetrieveTag(It.IsAny<string>())).Callback((string s) => postForModel.Tags.Add(new Tag(){ Name = s}));
        PostsRepoMock.Setup(r => r.CreatePost(It.IsAny<Post>())).Callback(() => postForModel.PostId = 1).ReturnsAsync(1);

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var result = await controller.CreateOrUpdatePost(postEditViewModel);
        
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(3, postEditViewModel.Post.Tags.Count);
        Assert.Equal(1, postEditViewModel.Post.PostId);
        Assert.Equal(postEditViewModel.Post.PostId, ((RedirectToActionResult)result).RouteValues?["postid"]);
        Assert.True(Enumerable.SequenceEqual(new string[]{"tag1", "tag2", "tag3"}, postEditViewModel.Post.Tags.Select(t => t.Name).ToArray()));
        PostsRepoMock.Verify(r => r.CreatePost(postForModel), Times.Once);
        PostsRepoMock.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Never);
    }

    [Fact]
    public async void AddOrUpdatePost_UpdatePost_ReturnsRedirectToPost()
    {
        Post postForModel = SeedPosts(1).First();
        postForModel.PostId = 7; //this post exists in the database, id is 7
        for(int i = 0; i < 5; i ++)
        {
            postForModel.Commentaries.Add( new Commentary() { CommentaryId = 1, Text = $"Text_{i}", Username = $"Username_{i}", Email = $"email_{i}@example.com" });
        }

        PostsRepoMock.Setup(r => r.CreateOrRetrieveTag(It.IsAny<string>())).Callback((string s) => postForModel.Tags.Add(new Tag {Name = s}));

        PostEditViewModel postEditViewModel = new() { Post = postForModel, TagString = "tag1,tag2,tag3"};

        HomeController controller = new(PostsRepoMock.Object, LoggerMock.Object, AccessorMock.Object, LinkGenMock.Object);

        var result = await controller.CreateOrUpdatePost(postEditViewModel);
        
        Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal(7, postEditViewModel.Post.PostId);
        Assert.Equal(postEditViewModel.Post.PostId, ((RedirectToActionResult)result).RouteValues?["postid"]);
        Assert.True(Enumerable.SequenceEqual(new string[]{"tag1", "tag2", "tag3"}, postEditViewModel.Post.Tags.Select(t => t.Name).ToArray()));
        PostsRepoMock.Verify(r => r.CreateOrRetrieveTag(It.IsAny<string>()), Times.Exactly(3));
        PostsRepoMock.Verify(r => r.CreatePost(postForModel), Times.Never);
        PostsRepoMock.Verify(r => r.UpdatePost(It.IsAny<Post>()), Times.Once);
    }






    private IEnumerable<Post> SeedPosts(int postNumber)
    {
        Post[] posts = new Post[postNumber];
        for (int i = 0; i < postNumber; i++)
        {
            Post post = new() { PostId = i, Header = $"Post_{i+1}", Text = new Guid().ToString() };
            posts[i] = post;
        }
        return posts;
    }

}