using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Tests.Helpers
{
    public static class SeedData
    {
        public static IEnumerable<Post> SeedPosts()
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
                },
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
                    Commentaries = commentariesList

                };
            };

            return posts;
        }
    }
}
