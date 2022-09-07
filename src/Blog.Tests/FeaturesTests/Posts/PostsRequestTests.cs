using Ardalis.Specification;
using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Features.Commentaries.Specifications;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Requests.Commands;
using Blog.Application.Features.Posts.Requests.Queries;
using Blog.Application.Features.Tags.Specifications;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using Blog.Tests.Helpers;
using Blog.Tests.Mocks;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Posts.Requests.Commands.CreatePostCommand;
using static Blog.Application.Features.Posts.Requests.Queries.GetPostByIdQuery;
using static Blog.Application.Features.Posts.Requests.Queries.GetPostToEditQuery;
using static Blog.Application.Features.Posts.Requests.Queries.ListPostsPageQuery;

namespace Blog.Tests.Features.Posts
{
    public class PostsRequestTests
    {
        private Mock<IBlogRepository<Post>> _postsRepoMock = RepoMocks.GetPostRepoMock();
        private Mock<IBlogRepository<Tag>> _tagsRepoMock = RepoMocks.GetTagsRepoMock();
        private Mock<IBlogRepository<Commentary>> _commentsRepoMock = RepoMocks.GetCommentsRepoMock();

        [Fact]
        public async Task ListPostsPageQuery_ReturnsCorrectVm()
        {
            //arrange

            var request = new ListPostsPageQuery(0, false);

            var commentRepoMock = new Mock<IBlogRepository<Commentary>>();
            commentRepoMock.Setup(r => r.CountAsync(It.IsAny<CommentariesByPostIdSpecification>())).ReturnsAsync(3);

            var mapper = MapperMock.GetMapperServiceMock(opts => opts.AddScoped(t => commentRepoMock.Object));

            var handler = new ListPostsPageQueryHandler(_postsRepoMock.Object, mapper);

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            Assert.Equal(5, result.PostsCount);
            Assert.Equal(3, result.Posts.First().CommentariesCount);
            Assert.Equal(1, result.PageCount);

        }
        [Fact]
        public async Task GetPostToEditQuery_ReturnsPostUpdateDto()
        {
            //arrange
            var repoMock = RepoMocks.GetPostRepoMock();
            var mapper = MapperMock.GetMapperServiceMock();

            var request = new GetPostToEditQuery(0);
            var handler = new GetPostToEditQueryHandler(_postsRepoMock.Object, mapper);

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Test1,Test2,Test3", result.TagString);
        }

        [Fact]
        public async Task CreatePostCommand_ReturnsCorrectId()
        {
            //arrange
            var post = new CreatePostDTO
            {
                Header = "Test Post",
                Text = "Created for the test",
                TagString = "existing,tag2,tag3"
            };


            var mapper = MapperMock.GetMapperServiceMock(opts => opts.AddScoped(t => _tagsRepoMock.Object));

            var request = new CreatePostCommand(post);
            var handler = new AddPostCommandHandler(_postsRepoMock.Object, mapper, Mock.Of<IValidator<CreatePostDTO>>());


            //act
            var result = await handler.Handle(request, CancellationToken.None);


            //assert
            Assert.Equal(RepoMocks.DEFAULT_CREATED_ID, result);

            _postsRepoMock.Verify(r =>
                r.CreateAsync(It.Is<Post>(p =>
                    p.Tags.Select(t => t.Name).SequenceEqual(new string[] { "existing", "tag2", "tag3" })
                    && post.Header == post.Header && post.Text == post.Text
                )), Times.Once);

            _tagsRepoMock.Verify(r => r.ListAsync(It.IsAny<TagsByTagNameSpecification>()), Times.Exactly(3));
        }

        [Fact]
        public async Task GetPostByIdQuery_ReturnsCorrectViewModel()
        {      
            //arrange
            var mapper = MapperMock.GetMapperServiceMock();
            var request = new GetPostByIdQuery(2, 0);
            var handler = new GetPostByIdQueryHandler(_postsRepoMock.Object, _commentsRepoMock.Object, mapper);

            //act
            var result =await  handler.Handle(request, CancellationToken.None);

            //assert
            Assert.Equal(3, result.Commentaries.Count);
            Assert.Equal(1, result.PageCount);
        }

        [Fact]
        public async Task GetPostByIdQuery_InvalidId_ThrowsException()
        {
            //arrange
            var mapper = MapperMock.GetMapperServiceMock();
            var request = new GetPostByIdQuery(9999, 0); //non existing ID
            var handler = new GetPostByIdQueryHandler(_postsRepoMock.Object, _commentsRepoMock.Object, mapper);

            //act

            //assert
            var ex =await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task GetPostToEditQuery_ReturnsCorrectDTO()
        {
            //arrange
            var mapper = MapperMock.GetMapperServiceMock(opts => opts.AddScoped<IBlogRepository<Tag>>(r => _tagsRepoMock.Object));
            var request = new GetPostToEditQuery(2);
            var handler = new GetPostToEditQueryHandler(_postsRepoMock.Object, mapper);

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            Assert.Equal("Test1,Test2,Test3", result.TagString);
            Assert.Equal("Header_2", result.Header);

        }

        [Fact]
        public async Task GetPostToEditQuery_InvalidId_ThrowsException()
        {
            //arrange
            var mapper = MapperMock.GetMapperServiceMock();
            var request = new GetPostToEditQuery(9999); //non existing ID
            var handler = new GetPostToEditQueryHandler(_postsRepoMock.Object, mapper);

            //act

            //assert
            var ex = await Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
        }

    }
}
