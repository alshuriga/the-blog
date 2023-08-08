using Blog.Application.Constants;
using Blog.Core.Entities;
using Blog.Infrastructure.Data;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;

namespace Blog.Infrastructure.DataSeed;

public static class SeedData
{
    public static void EnsureSeedContent(IServiceProvider services)
    {
        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<BlogEFContext>();
        appDb.Database.EnsureCreated();

        if (appDb.Posts.Any()) return;

        var posts = new Post[]
        {
           new Post()
           {
               Header = "Exploring the World of 🚀 Space Tourism",
               Text = "Space tourism is on the rise! 🌌✨ Imagine floating in zero gravity, gazing at Earth from the window of a space capsule. Organizations like NASA and Blue Origin are making this dream a reality. 🛰️🪐 Would you dare to take a trip to the stars?",
               DateTime = DateTime.Now - TimeSpan.FromMinutes(new Random().Next(200)),
               IsDraft = false
           },
           new Post()
           {
               Header = "Embracing Mindfulness: 🧘‍♂️ Finding Peace in a Hectic World",
               Text =  "In our fast-paced lives, finding moments of tranquility is essential. 🌿🌼 Practicing mindfulness allows us to be present, reduce stress, and appreciate life's simple joys. Whether through meditation, deep breathing, or mindful walks, taking time for yourself is a gift you deserve. 🌸🙏",
               DateTime = DateTime.Now - TimeSpan.FromMinutes(new Random().Next(200)),
               IsDraft = false
           },
            new Post()
           {
               Header = "🍔 The Art of Crafting Gourmet Burgers",
               Text =  "Burgers aren't just fast food – they can be gourmet creations too! 🍔🍔 From unique patty blends to artisanal buns and exotic toppings, burger enthusiasts are taking this classic comfort food to new heights. Let's explore some mouthwatering burger combinations that will leave you craving for more!",
               DateTime = DateTime.Now - TimeSpan.FromMinutes(new Random().Next(200)),
               IsDraft = false
           },
           new Post()
           {
               Header = "🎬 The Evolution of Special Effects in Cinema",
               Text =   "From practical effects to cutting-edge CGI, the world of cinema has come a long way in creating mesmerizing visuals. 🎥🌟 Early filmmakers used miniatures and painted backdrops, while today's blockbusters use technology to transport us to other worlds. Let's take a journey through the history of movie magic!",
               DateTime = DateTime.Now - TimeSpan.FromMinutes(new Random().Next(200)),
               IsDraft = false
           },

            new Post()
           {
               Header =  "📚 The Joy of Reading: Escaping into Different Realms",
               Text =    "Books have the remarkable ability to transport us to new worlds, cultures, and perspectives. 📖✨ Whether you're delving into fantasy realms, exploring historical eras, or solving mysteries, reading opens the door to endless adventures. Let's celebrate the magic of books and the joy of getting lost in their pages!",
               DateTime = DateTime.Now - TimeSpan.FromMinutes(new Random().Next(200)),
               IsDraft = false
           },
        };
         List<string> neutralCommentaries = new List<string>
        {
            "The weather has been quite unpredictable lately. ☔️☀️",
            "I tried a new recipe last night, and it turned out surprisingly well. 🍽️👌",
            "I'm thinking of redecorating my living room soon. 🏠✨",
            "Did you catch that new movie that came out last weekend? 🎬🍿",
            "I can't believe how fast this year is flying by. 📆✈️",
            "I've been getting into gardening lately – it's quite therapeutic. 🌱🌼",
            "I'm excited for the upcoming holiday season and all the festivities. 🎉🎄",
            "Have you heard about the new art exhibit opening downtown? 🎨🖼️",
            "I've been binge-watching a new TV series, and it's quite addictive. 📺😬",
            "Traffic was surprisingly light during my morning commute today. 🚗🛣️",
            "I'm planning a weekend getaway to unwind and relax. 🏖️🌴",
            "I've been trying to cut back on caffeine lately, but it's a struggle. ☕️😅",
            "I've started learning a new musical instrument – it's challenging but fun. 🎶🎹",
            "Have you been following the latest sports events? Some exciting games recently. ⚽️🏀",
            "I can't decide what book to read next – so many options! 📚🤔",
            "I recently attended a virtual conference, and it was surprisingly engaging. 💻🎤",
            "I'm considering taking up hiking – it seems like a great way to stay active. 🚶‍♂️🌲",
            "I've been experimenting with new cooking techniques in the kitchen. 👨‍🍳🍳",
            "I went to a local farmer's market over the weekend and discovered some amazing products. 🛒🥦",
            "I'm contemplating adopting a pet – it's a big decision to make. 🐶🐱",
        };

        List<string> exampleTags = new List<string>
        {
            "technology",
            "travel",
            "food",
            "health",
            "fitness",
            "art",
            "books",
            "music",
            "movies",
            "science",
            "nature",
            "photography",
            "fashion",
            "cooking",
            "history",
            "sports",
            "hobbies",
            "finance",
            "culture",
            "education",
        };

        var tags = new List<Tag>();
        foreach (var et in exampleTags) tags.Add(new Tag() { Name = et });

        foreach(Post p in posts)
        {
            var random = new Random();
            p.Commentaries = new List<Commentary>();
            var commentCount = random.Next(0, 7);
            for (int i = 0; i < commentCount; i++)
                p.Commentaries.Add(new Commentary()
                    { Text = neutralCommentaries[random.Next(0, neutralCommentaries.Count - 1)],
                    DateTime = p.DateTime + TimeSpan.FromMinutes(random.Next(100)),
                    Username = $"User{random.Next(1000)}",
                    Email = "example@mail.com" });

            p.Tags = new List<Tag>();
            var tagCount = random.Next(3, 10);
            for (int i = 0; i < tagCount; i++)
            {
                var tagIndex = random.Next(0, tags.Count() - 1);
                if (!p.Tags.Contains(tags[tagIndex]))
                    p.Tags.Add(tags[tagIndex]);
            }

            var likesCount = random.Next(0, 100);
            for(int i = 0; i < likesCount; i++)
            {
                p.Likes.Add(new Like() { Username = $"User_{random.Next(999)}", DateTime = DateTime.Now });
            }
            appDb.Posts.Add(p);   
        }
        appDb.SaveChanges();
    }




    public static async Task EnsureSeedIdentity(IServiceProvider services)
    {

        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<IdentityEFContext>();


        appDb.Database.EnsureCreated();

        services = services.CreateScope().ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.Roles.Any() && !userManager.Users.Any())
        {
            IdentityRole adminsRole = new IdentityRole(RolesConstants.ADMIN_ROLE);
            await roleManager.CreateAsync(adminsRole);

            IdentityUser adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            await userManager.CreateAsync(adminUser, "admin");

            await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin"), adminsRole.Name);


            IdentityUser normalUser = new IdentityUser
            {
                UserName = "normal",
                Email = "normal@basic.com"
            };
            await userManager.CreateAsync(normalUser, "12345");
        }

    }
}