using Blog.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    public class BlogEFContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Commentary> Commentaries => Set<Commentary>();
        public DbSet<Tag> Tags => Set<Tag>();

        public BlogEFContext(DbContextOptions<BlogEFContext> options) : base(options) { }
    }

}
