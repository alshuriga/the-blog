using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;
using MiniBlog.Infrastructure.DataSeed;

namespace MiniBlog.Infrastructure.DataSeed;

public static class SeedData
{
    public static void EnsureSeed(IServiceProvider services)
    {
        var db = services.CreateScope().ServiceProvider.GetRequiredService<MiniBlogEfContext>();
        
        if (db.Database.GetPendingMigrations().Any())
        {
            db.Database.Migrate();
        }

        if (!db.Posts.Any())
        {
            Post[] posts = new Post[]
            {
                new() {
                    Header = "Hello!",
                    Text = "This is my first post.",
                    Tags = new List<Tag>() {
                     new() { Name = "test"}, new() {Name = "greetings"}
                    }
                },
            };
            db.Posts?.AddRange(posts);
            db.SaveChanges();
        }

    }
}
