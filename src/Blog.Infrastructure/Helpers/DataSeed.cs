using Blog.Application.Constants;
using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;

namespace Blog.Infrastructure.Helpers;

public static class DataSeed
{
    public static IServiceProvider EnsureDataCreatedAndSeeded(this IServiceProvider services)
    {
        var db = services.GetRequiredService<BlogEFContext>();
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

        return services;

    }

    public static IServiceProvider EnsureIdentityCreatedAndSeeded(this IServiceProvider services)
    {
        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<IdentityEFContext>();


        appDb.Database.EnsureCreated();

        services = services.CreateScope().ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.Roles.Any() && !userManager.Users.Any())
        {
            IdentityRole adminsRole = new IdentityRole(RolesConstants.ADMIN_ROLE);
            roleManager.CreateAsync(adminsRole).Wait();

            IdentityUser adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            userManager.CreateAsync(adminUser, "admin").Wait();

            userManager.AddToRoleAsync(userManager.FindByNameAsync("admin").Result, adminsRole.Name).Wait();

            IdentityUser normalUser = new IdentityUser
            {
                UserName = "normal",
                Email = "normal@basic.com"
            };
            userManager.CreateAsync(normalUser, "12345").Wait();

        }

        return services;
    }
}
