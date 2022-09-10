using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IntegrationTests.MVC.Mocks
{
    public static class DataSeed
    {
        public static void EnsureCreatedAndSeeded(this BlogEFContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            Tag[] tags = new Tag[]
            {
                new Tag
                {
                    Id = 1,
                    Name = "one"
                },
                new Tag
                {
                    Id = 2,
                    Name = "two"
                },
                new Tag
                {
                    Id = 3,
                    Name = "three"
                }
            };

            Post[] posts = new Post[]
            {
               new Post
               {
                   Id = 1,
                   Header = "Post 1",
                   Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                   Tags = tags,
                   Commentaries = new Commentary[]
                   {
                       new Commentary
                       {
                           Id = 1,
                           Text = "This is the test commentary.",
                           Username = "Tester1",
                           Email = "tester1@test.com",
                       }
                   },
                   IsDraft = false,
                   DateTime = DateTime.MinValue + TimeSpan.FromMinutes(1)
                },

               new Post
               {
                   Id = 2,
                   Header = "Post 2",
                   Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                   Tags = tags,
                   Commentaries = new Commentary[]
                   {
                       new Commentary
                       {
                           Id = 2,
                           Text = "This is the test commentary.",
                           Username = "Tester2",
                           Email = "tester2@test.com",
                       }
                   },
                   IsDraft = false,
                   DateTime = DateTime.MinValue + TimeSpan.FromMinutes(2)
                },

                new Post
               {
                   Id = 3,
                   Header = "Draft 1",
                   Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                   Tags = tags,
                   IsDraft = true,
                   DateTime = DateTime.MinValue + TimeSpan.FromMinutes(3)
                },
                 new Post
               {
                   Id = 4,
                   Header = "Draft 2",
                   Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.",
                   Tags = tags,
                   IsDraft = true,
                   DateTime = DateTime.MinValue + TimeSpan.FromMinutes(4)
                }
           };

            db.Tags.AddRange(tags);
            db.Posts.AddRange(posts);

            db.SaveChanges();

        }
    }
}
