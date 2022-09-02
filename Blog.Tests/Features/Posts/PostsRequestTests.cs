using Ardalis.Specification;
using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Requests.Commands;
using Blog.Application.Features.Posts.Requests.Queries;
using Blog.Application.Features.Tags.Specifications;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using Blog.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Blog.Application.Features.Posts.Requests.Commands.CreatePostCommand;
using static Blog.Application.Features.Posts.Requests.Queries.GetPostToEditQuery;
using static Blog.Application.Features.Posts.Requests.Queries.ListPostsPageQuery;

namespace Blog.Tests.Features.Posts
{
    public class PostsRequestTests
    {
        [Fact]
        public async Task ListPostsPageQuery_ReturnsCorrectVm()
        {
            //arrange
            var postsList = SeedData.SeedPosts();

            var repoMock = new Mock<IBlogRepository<Post>>();
            repoMock.Setup(r => r.ListAsync(It.IsAny<ISpecification<Post>>())).ReturnsAsync(postsList);
            repoMock.Setup(r => r.CountAsync(null)).ReturnsAsync(postsList.Count);

            var mapper = MockMapper.GetMapperServiceMock();

            var request = new ListPostsPageQuery() { CurrentPage = 0 };
            var handler = new ListPostsPageQueryHandler(repoMock.Object, mapper);

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            Assert.Equal(10, result.PostsCount);
            Assert.Equal(3, result.Posts.First().CommentariesCount);
            Assert.True(postsList.Select(p => p.Header).SequenceEqual(result.Posts.Select(p => p.Header)));

        }
        [Fact]
        public async Task GetPostToEditQuery_ReturnsPostUpdateDto()
        {
            //arrange
            var post = SeedData.SeedPosts().First();
            var repoMock = new Mock<IBlogRepository<Post>>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(post);

            var mapper = MockMapper.GetMapperServiceMock();

            var request = new GetPostToEditQuery() { Id = 0 };
            var handler = new GetPostToEditQueryHandler(repoMock.Object, mapper);

            //act
            var result = await handler.Handle(request, CancellationToken.None);

            //assert
            Assert.Equal(post.DateTime, result.DateTime);
            Assert.Equal(String.Join(",", post.Tags.Select(t => t.Name)), result.TagString);
        }

        [Fact]
        public async Task AddPost_ReturnsCorrectDto()
        {
            //arrange
            var post = new CreatePostDTO
            {
                Header = "Test Post",
                Text = "Created for the test",
                DateTime = DateTime.Now,
                TagString = "tag1,tag2,tag3"
            };

            Post? createdPost = null;

            var repoMock = new Mock<IBlogRepository<Post>>();
            repoMock.Setup(r => r.CreateAsync(It.IsAny<Post>())).ReturnsAsync(5).Callback<Post>(r => createdPost = r);

            var tagRepoMock = new Mock<IBlogRepository<Tag>>();
            tagRepoMock.SetupSequence(r => r.ListAsync(It.IsAny<TagsByTagNameSpecification>()))
                .ReturnsAsync(new Tag[] { new Tag { Id = 25, Name = "existing" } })
                .ReturnsAsync(Array.Empty<Tag>())
                .ReturnsAsync((Array.Empty<Tag>()));

            var mapper = MockMapper.GetMapperServiceMock(conf =>
            {
                conf.AddTransient<IBlogRepository<Tag>>(r => tagRepoMock.Object);
            });

            var request = new CreatePostCommand() { PostDTO = post };
            var handler = new AddPostCommandHandler(repoMock.Object, mapper);


            //act
            var result = await handler.Handle(request, CancellationToken.None);


            //assert
            Assert.Equal(5, result);
            Assert.Equal(new string[] {"existing", "tag2", "tag3" }, createdPost?.Tags.Select(t => t.Name).ToArray());
            Assert.Equal(new long[] { 25, 0, 0 }, createdPost?.Tags.Select(t => t.Id).ToArray());
            Assert.Equal(post.DateTime, createdPost?.DateTime);
            tagRepoMock.Verify(r => r.ListAsync(It.IsAny<TagsByTagNameSpecification>()), Times.Exactly(3));

        }

    }
}
