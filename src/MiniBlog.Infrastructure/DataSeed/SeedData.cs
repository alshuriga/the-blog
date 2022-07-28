using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;

namespace MiniBlog.Infrastructure;

public static class SeedData
{
    public static void EnsureSeed(MiniBlogDbContext db)
    {
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
