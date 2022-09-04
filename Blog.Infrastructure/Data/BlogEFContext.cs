using Blog.Core.Entities;
using Blog.Core.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    public class BlogEFContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Commentary> Commentaries => Set<Commentary>();
        public DbSet<Tag> Tags => Set<Tag>();

        public BlogEFContext(DbContextOptions<BlogEFContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Navigation(p => p.Tags).AutoInclude();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            
           foreach(var entry in ChangeTracker.Entries())
            {
                if(entry.State == EntityState.Added && (entry.Entity is AddibleEntity addible))
                {
                    addible.DateTime = DateTime.Now;
                }
            }
            return base.SaveChangesAsync();
        }
    }

}
