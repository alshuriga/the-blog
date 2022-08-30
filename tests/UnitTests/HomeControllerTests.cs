using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Web.Pages;
using Microsoft.AspNetCore.Identity;
using MiniBlog.Tests.UnitTests.Mocks;
using MiniBlog.Web.ViewModels;
using Newtonsoft.Json;
using MiniBlog.Core.Constants;
using Ardalis.Specification;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MiniBlog.Core.Specifications;
using System.Linq.Expressions;

namespace MiniBlog.Tests.UnitTests;


public class HomeControllersTests
{
    private Mock<UserManager<IdentityUser>> UserManagerMock { get; set; } = new(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

    private Mock<IUnitOfWork> unitMock;
    private Mock<IRepository<Post>> postRepo = new();
    private Mock<IRepository<Commentary>> commentaryRepo = new();
    private Mock<IRepository<Tag>> tagRepo = new();
    private Mock<IReadRepository<Post>> postReadRepo = new();
    private Mock<IReadRepository<Commentary>> commentaryReadRepo = new();
    private Mock<IReadRepository<Tag>> tagReadRepo = new();
    private int SkipPostToPage(int page) => page == 1 ? 0 : PaginationConstants.POSTS_PER_PAGE * (page - 1);

    public HomeControllersTests()
    {
        unitMock = new Mock<IUnitOfWork>();
        unitMock.SetupGet(u => u.postRepo).Returns(postRepo.Object);
        unitMock.SetupGet(u => u.commentRepo).Returns(commentaryRepo.Object);
        unitMock.SetupGet(u => u.tagsRepo).Returns(tagRepo.Object);
        unitMock.SetupGet(u => u.postReadRepo).Returns(postReadRepo.Object);
        unitMock.SetupGet(u => u.commentReadRepo).Returns(commentaryReadRepo.Object);
        unitMock.SetupGet(u => u.tagsReadRepo).Returns(tagReadRepo.Object);
    }


    [Fact]
    public async void List_SecondPageOfPosts_ReturnsViewWithCorrectPostsModel()
    {
        var mockPosts = SeedTestData.Posts(PaginationConstants.POSTS_PER_PAGE * 3);
        var expectedPosts = mockPosts.Skip(SkipPostToPage(3)).Take(PaginationConstants.POSTS_PER_PAGE);
        postReadRepo.Setup(p => p.ListAsync(It.IsAny<Specification<Post>>())).ReturnsAsync(expectedPosts);
        postReadRepo.Setup(p => p.CountAsync(It.IsAny<Specification<Post>>())).ReturnsAsync(mockPosts.Count());
        var expectedPostDto = expectedPosts.Select(p => new PostDto
        {
            Id = p.Id,
            Header = p.Header,
            Text = p.Text,
            DateTime = p.DateTime
        }).ToArray();

        var expectedTags = expectedPosts.Select(p => p.Tags.Select(t => t.Name));

        var commentsCount = expectedPosts.Select(p => p.Commentaries.Count);

        var model = new ListModel(unitMock.Object);

        await model.OnGet(currentPage: 2);
        var actualPosts = model.Posts.Select(p => p.Post).ToArray();

        Assert.Equal(JsonConvert.SerializeObject(expectedPostDto), JsonConvert.SerializeObject(actualPosts));
        Assert.Equal(mockPosts.Count(), model.PostsCount);
        Assert.Equal(PaginationConstants.POSTS_PER_PAGE, model.Posts.Count());
        Assert.True(Enumerable.SequenceEqual(commentsCount, model.Posts.Select(p => p.CommentariesCount)));
        Assert.True(Enumerable.SequenceEqual(JsonConvert.SerializeObject(expectedTags), JsonConvert.SerializeObject(model.Posts.Select(p => p.TagNames))));
        Assert.False(model.Posts.Where(p => p.CommentsButton == false).Any());
        Assert.True(model.PaginationData != null);
    }


    [Fact]
    public async void Index_GetLessPostsThanPerPage_ReturnsViewWithNullPagination()
    {
        var expectedPosts = SeedTestData.Posts(PaginationConstants.POSTS_PER_PAGE - 2);
        postReadRepo.Setup(p => p.ListAsync(It.IsAny<Specification<Post>>()))
            .ReturnsAsync(expectedPosts);
        var commentReadRepoMock = new Mock<IReadRepository<Commentary>>();
        commentReadRepoMock.Setup(r => r.CountAsync(It.IsAny<Specification<Commentary>>())).ReturnsAsync(5);

        unitMock.Setup(u => u.postReadRepo).Returns(postReadRepo.Object);
        unitMock.Setup(u => u.commentReadRepo).Returns(commentReadRepoMock.Object);

        var expectedPostDto = expectedPosts.Select(p => new PostDto
        {
            Id = p.Id,
            Header = p.Header,
            Text = p.Text,
            DateTime = p.DateTime
        }).ToArray();

        var model = new ListModel(unitMock.Object);

        await model.OnGet(currentPage: 1);
        var actualPostsDto = model.Posts.Select(p => p.Post).ToArray();

        postReadRepo.Verify(p => p.ListAsync(It.IsAny<Specification<Post>>()), Times.Once);
        postReadRepo.Verify(p => p.ListAsync(It.Is<PostsByPageSpecification>(s => s.Evaluate(expectedPosts).Count() == expectedPosts.Count())), Times.Once);
        Assert.False(model.Posts.Where(p => p.CommentariesCount != 5).Any());
        Assert.Equal(JsonConvert.SerializeObject(expectedPostDto), JsonConvert.SerializeObject(actualPostsDto));
        Assert.True(model.PaginationData == null);
    }

    [Fact]
    public async void Index_RepoHasMultiplePosts_ReturnsCorrectPagesNumber()
    {
        var expectedPageNumber = 5;
        var mockPosts = SeedTestData.Posts(PaginationConstants.POSTS_PER_PAGE * expectedPageNumber);
        var pageOfPosts = mockPosts.Take(PaginationConstants.POSTS_PER_PAGE);
        postReadRepo.Setup(r => r.CountAsync(It.IsAny<ISpecification<Post>>())).ReturnsAsync(mockPosts.Count());
        postReadRepo.Setup(r => r.ListAsync(It.IsAny<Specification<Post>>())).ReturnsAsync(pageOfPosts);

        var model = new ListModel(unitMock.Object);

        await model.OnGet(currentPage: 1);

        Assert.Equal(expectedPageNumber, model.PaginationData?.PagesCount);
    }

    [Fact]
    public async void Index_GetPostsByTag_ReturnsCorrectPosts()
    {
        int expectedPostsNumber = 3;
        var testTag = new Tag { Name = "test1234" };
        var postsMock = SeedTestData.Posts(10);

        foreach (var post in postsMock.Take(expectedPostsNumber - 1))
        {
            post.Tags.Add(testTag);
        }

        var expectedPosts = postsMock.Take(expectedPostsNumber);
        Expression<Func<PostsByPageSpecification, bool>> func = p =>
        !p.Evaluate(postsMock)
            .Select(p => p.Tags)
            .Select(t => t.Select(t => t.Name))
            .Where(t => !t.Contains(testTag.Name))
            .Any();

        var model = new ListModel(unitMock.Object);
        await model.OnGet(currentPage: 1, testTag.Name);


        postReadRepo.Verify(p => p.ListAsync(It.IsAny<Specification<Post>>()), Times.Once);
        postReadRepo.Verify(p => p.ListAsync(It.Is<PostsByPageSpecification>(func)), Times.Once);
    }


    [Fact]
    public async void Post_GetPostById_ReturnsViewWithCorrectPost()
    {
        Post post = SeedTestData.Posts(1).First();
        postReadRepo.Setup(p => p.RetrieveByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Post, object>>[]?>())).ReturnsAsync(post);
        var model = new SinglePostModel(unitMock.Object, UserManagerMock.Object);
        model.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
        await model.OnGet(post.Id);
        Assert.Equal(post.Id, model.PostPartial.Post.Id);
        Assert.True(model.Commentaries.Count() <= PaginationConstants.COMMENTS_PER_PAGE);
        Assert.Equal(post.Id.ToString(), model.TempData["postId"]);
        Assert.False(model.PostPartial.CommentsButton);
        Assert.True(post.Tags.Select(t => t.Name).SequenceEqual(model.PostPartial.TagNames));

    }


    [Fact]
    public async void Post_GetPostByInvalidId_ReturnsNotFound()
    {
        postReadRepo.Setup(p => p.RetrieveByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Post, object>>[]?>())).ReturnsAsync((Post?)null).Verifiable();

        var model = new SinglePostModel(unitMock.Object, UserManagerMock.Object);

        var result = await model.OnGet(1);

        Assert.IsType<NotFoundResult>(result);
        postReadRepo.Verify(p => p.RetrieveByIdAsync(1, It.IsAny<Expression<Func<Post, object>>[]?>()), Times.Once());
        postReadRepo.VerifyNoOtherCalls();

    }

    [Fact]
    public async void SavePost_AddPostAsAdmin_ReturnsRedirectToPost()
    {
        PostDto postDto = new()
        {
            Id = 0,
            Header = new Guid().ToString(),
            Text = "This is a new post",
            DateTime = DateTime.Now
        };
        string tagString = "tag1,tag2,tag3";
        postReadRepo.Setup(r => r.RetrieveByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Post, object>>[]?>())).ReturnsAsync((Post?)null);
        tagReadRepo.Setup(r => r.ListAsync(It.IsAny<TagsByNameSpecification>())).ReturnsAsync(Enumerable.Empty<Tag>());
        var model = new EditorModel(unitMock.Object);
        model.PageContext = MockObjects.GetPageContext(true, RolesConstants.ADMIN_ROLE);
        var result = await model.OnPost(postDto, tagString);
        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("SinglePost", (result as RedirectToPageResult)?.PageName);
        postRepo.Verify(p => p.AddAsync(It.Is<Post>(p => 
        p.Id == postDto.Id && p.Header == postDto.Header 
        && p.DateTime == postDto.DateTime && p.Text == postDto.Text
        )), Times.Once);
        postRepo.Verify(p => p.UpdateAsync(It.IsAny<Post>()), Times.Never);

    }

    [Fact]
    public async void SavePost_UpdateAsAdmin_ReturnsRedirectToPost()
    {
        Post oldPost = new()
        {
            Id = 5,
            Header = "Old header",
            Text = "Old text",
            DateTime = DateTime.MinValue
        };

        PostDto postDto = new()
        {
            Id = 5,
            Header = new Guid().ToString(),
            Text = "This is a new post",
            DateTime = DateTime.Now
        };
        string tagString = "tag1,tag2,tag3";
           postReadRepo.Setup(r => r.RetrieveByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Post, object>>[]?>())).ReturnsAsync(oldPost);
           tagReadRepo.Setup(r => r.ListAsync(It.IsAny<TagsByNameSpecification>())).ReturnsAsync(Enumerable.Empty<Tag>());
        var model = new EditorModel(unitMock.Object);
        model.PageContext = MockObjects.GetPageContext(true, RolesConstants.ADMIN_ROLE);

        var result = await model.OnPost(postDto, tagString);

        Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("SinglePost", (result as RedirectToPageResult)?.PageName);
        Assert.Equal(oldPost.Id, (result as RedirectToPageResult)!.RouteValues!["postId"]);
        postRepo.Verify(p => p.UpdateAsync(It.Is<Post>(p =>
            p.Header == postDto.Header
            && p.DateTime == postDto.DateTime 
            && p.Text == postDto.Text)), 
            Times.Once);
        postRepo.Verify(p => p.AddAsync(It.IsAny<Post>()), Times.Never);
     
    }
}


//    [Fact]
//    public async void EditPost_PassExistingPostId_ReturnsViewForUpdating()
//    {
//        Post post = new Post() { PostId = 1, Header = "Test", Text = "test", DateTime = DateTime.Now };
//        post.Tags = new List<Tag>()
//         {
//             new Tag() { TagId = 1, Name = "test1" },
//             new Tag() { TagId = 2, Name = "test2" },
//             new Tag() { TagId = 3, Name = "test3" }
//         };
//        string tagString_expected = "test1,test2,test3";
//        PostsRepoMock.Setup(r => r.RetrievePost(1, It.IsAny<PaginateParams>())).ReturnsAsync(post);

//        var controller = GetTestControllerWithMocks();
//        var result = await controller.EditPost(1);
//        var resultModel = (result as ViewResult)?.Model as PostEditViewModel;

//        Assert.IsType<ViewResult>(result);
//        Assert.NotNull(resultModel);
//        Assert.Equal(tagString_expected, resultModel?.TagString);
//        Assert.Equal(3, resultModel?.Post.Tags.Count);
//        Assert.Equal(1, resultModel?.Post.PostId);
//        Assert.Equal("Edit Post", (result as ViewResult)?.ViewData["title"]);
//    }


//    [Fact]
//    public async void EditPost_PassNoPostId_ReturnsViewForCreating()
//    {
//        PostsRepoMock.Setup(r => r.RetrievePost(0, It.IsAny<PaginateParams>())).ReturnsAsync((Post?)null);
//        var controller = GetTestControllerWithMocks();
//        var result = await controller.EditPost();
//        var resultModel = (result as ViewResult)?.Model as PostEditViewModel;
//        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Never);
//        Assert.IsType<ViewResult>(result);
//        Assert.NotNull(resultModel);
//        Assert.Equal(0, resultModel?.Post.PostId);
//        Assert.Equal("New Post", (result as ViewResult)?.ViewData["title"]);
//    }

//    [Fact]
//    public async void EditPost_PassNonexistingPostId_ReturnsNotFound()
//    {
//        PostsRepoMock.Setup(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()))
//            .ReturnsAsync((Post?)null);

//        var controller = GetTestControllerWithMocks();
//        var result = await controller.EditPost(245); //non-existing post id

//        PostsRepoMock.Verify(r => r.RetrievePost(It.IsAny<long>(), It.IsAny<PaginateParams>()), Times.Once);
//        Assert.IsType<NotFoundResult>(result);
//    }

//    [Fact]
//    public async void DeletePost_PassExistingPostId_ReturnsRedirectToIndex()
//    {
//        long postId = 15;
//        var controller = GetTestControllerWithMocks();

//        var result = await controller.DeletePost(postId);

//        PostsRepoMock.Verify(r => r.DeletePost(It.Is<long>(l => l == postId)), Times.Once);
//        Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
//    }

//    [Fact]
//    public async void AddComment_AuthenticatedUser_AddsCommentAndRedirectsToPost()
//    {
//        // Given
//        var userName = "TestUserName";
//        var postId = 10;
//        var email = "test-email@test.com";
//        var controller = GetTestControllerWithMocks();
//        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
//             new Claim (ClaimTypes.NameIdentifier, "NameIdentifier"),
//             new Claim(ClaimTypes.Name, userName)
//         }, "TestIdentity"));
//        var testUser = new IdentityUser { UserName = userName, Email = email };
//        UserManagerMock.Setup(m => m.FindByNameAsync(It.Is<string>(s => s == userName))).ReturnsAsync(testUser);
//        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

//        var testCommentary = new CommentaryViewModel { Text = "This is a test commentary" };
//        // When
//        var result = await controller.AddComment(testCommentary, postId);
//        // Then    
//        UserManagerMock.Verify(m => m.FindByNameAsync(It.Is<string>(s => s == userName)), Times.Once);
//        PostsRepoMock.Verify(r => r.CreateComment(
//            It.Is<Commentary>(c => c.Text == testCommentary.Text
//            && c.Username == testUser.UserName
//            && c.Email == testUser.Email),
//            It.Is<long>(l => l == postId)), Times.Once);
//        Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal(postId, (long?)((RedirectToActionResult)result).RouteValues?["postId"]);
//    }


//    [Fact]
//    public async void DeleteComment_PassExistingCommentId_RedirectsToPost()
//    {
//        var returnId = 1;
//        var commId = 12;
//        var controller = GetTestControllerWithMocks();

//        var result = await controller.DeleteComment(commId, returnId);

//        PostsRepoMock.Verify(r => r.DeleteComment(It.Is<long>(l => ((byte)l) == commId)), Times.Once);
//        Assert.IsType<RedirectToActionResult>(result);
//        Assert.Equal(returnId, (long?)((RedirectToActionResult)result).RouteValues?["postId"]);
//    }

//    private IEnumerable<Post> SeedPosts(int postNumber)
//    {
//        Post[] posts = new Post[postNumber];
//        for (int i = 0; i < postNumber; i++)
//        {
//            Post post = new() { PostId = i + 1, Header = $"Post_{i + 1}", Text = new Guid().ToString() };
//            posts[i] = post;
//        }

//        return posts;
//    }


//}