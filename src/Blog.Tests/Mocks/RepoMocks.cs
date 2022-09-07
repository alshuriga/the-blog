using Ardalis.Specification;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Tests.Mocks;

public static class RepoMocks
{
    public const long DEFAULT_CREATED_ID = 1234;

    public static Mock<IBlogRepository<Post>> GetPostRepoMock()
    {
        var postRepoMock = new Mock<IBlogRepository<Post>>();
        postRepoMock.Setup(r => r.CreateAsync(It.IsAny<Post>())).ReturnsAsync(DEFAULT_CREATED_ID);
        postRepoMock.Setup(r => r.ListAsync(It.IsAny<ISpecification<Post>>())).ReturnsAsync((ISpecification<Post> spec) => spec.Evaluate(SeedPosts()));
        postRepoMock.Setup(r => r.CountAsync(It.IsAny<ISpecification<Post>>())).ReturnsAsync((ISpecification<Post> spec) => spec.Evaluate(SeedPosts()).Count());
        postRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((long id) => SeedPosts().Where(p => p.Id == id).FirstOrDefault());
        postRepoMock.Setup(r => r.DeleteAsync(It.IsAny<long>())).Callback((long id) => SeedPosts().Remove(SeedPosts().Where(p => p.Id == id).FirstOrDefault()!));
        postRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Post>())).Callback((Post upd) =>
        {
            var post = SeedPosts().Where(p => p.Id == upd.Id).FirstOrDefault();
            if (post != null)
            {
                post.Header = upd.Header;
                post.Text = upd.Text;
                post.Tags = upd.Tags;
            }
        });

        return postRepoMock;
    }

    public static Mock<IBlogRepository<Commentary>> GetCommentsRepoMock()
    {
        var posts = SeedPosts();
        List<Commentary> commentsList = new();
        foreach (var post in posts)
        {
            commentsList.AddRange(new Commentary[]
        {
                new Commentary
                {
                    Email = "email1@test.com",
                    Username = "username1@test.com",
                    DateTime = DateTime.MinValue + TimeSpan.FromDays(1),
                    Text = "This is a test comment number 1.",
                    PostId = post.Id

                },
            new Commentary
            {
                Email = "email2@test.com",
                Username = "username2@test.com",
                DateTime = DateTime.MinValue + TimeSpan.FromDays(2),
                Text = "This is a test comment number 2.",
                PostId = post.Id
            },
            new Commentary
            {
                Email = "email3@test.com",
                Username = "username3@test.com",
                DateTime = DateTime.MinValue + TimeSpan.FromDays(1),
                Text = "This is a test comment number 3.",
                PostId = post.Id

            }
        });
        }

        var commentsRepoMock = new Mock<IBlogRepository<Commentary>>();
        commentsRepoMock.Setup(r => r.ListAsync(It.IsAny<ISpecification<Commentary>>())).ReturnsAsync((ISpecification<Commentary> spec) => spec.Evaluate(commentsList));
        commentsRepoMock.Setup(r => r.CountAsync(It.IsAny<ISpecification<Commentary>>())).ReturnsAsync((ISpecification<Commentary> spec) => spec.Evaluate(commentsList).Count());

        return commentsRepoMock;
    }



    public static Mock<IBlogRepository<Tag>> GetTagsRepoMock()
    {
        List<Tag> tagsList = new() {
            new Tag { Id = 123, Name = "existing" }
        };

        var tagsRepoMock = new Mock<IBlogRepository<Tag>>();

        tagsRepoMock.Setup(r => r.ListAsync(It.IsAny<ISpecification<Tag>>())).ReturnsAsync((ISpecification<Tag> spec) => spec.Evaluate(tagsList));

        return tagsRepoMock;

    }

    private static List<Post> SeedPosts()
    {
        Post[] posts = new Post[10];

        Tag[] tagsList = {
            new Tag { Id = 1, Name = $"Test1" } ,
            new Tag { Id = 2, Name = $"Test2" },
            new Tag { Id = 3, Name = $"Test3" }
        };

        Commentary[] commentariesList =
        {
            new Commentary
            {
                Email = "email1@test.com",
                Username = "username1@test.com",
                DateTime = DateTime.MinValue + TimeSpan.FromDays(1),
                Text = "This is a test comment number 1."

            },
            new Commentary
            {
                Email = "email2@test.com",
                Username = "username2@test.com",
                DateTime = DateTime.MinValue + TimeSpan.FromDays(2),
                Text = "This is a test comment number 2."
            },
            new Commentary
            {
                Email = "email3@test.com",
                Username = "username3@test.com",
                DateTime = DateTime.MinValue + TimeSpan.FromDays(1),
                Text = "This is a test comment number 3."
            }
        };

        var postsList = new List<Post>();

        for (int i = 0; i < 10; i++)
        {
            posts[i] = new Post
            {
                Id = i,
                Header = $"Header_{i}",
                Text = $"Text_{i}",
                Tags = tagsList,
                DateTime = DateTime.MinValue,
                Commentaries = commentariesList,
                IsDraft = false

            };
        };

        for (int i = 0; i < 5; i++)
        {
            posts[i].IsDraft = true;
        }

        return posts.ToList();
    }
}
