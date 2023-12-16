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
        public DbSet<Like> Like => Set<Like>();

        public BlogEFContext(DbContextOptions<BlogEFContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>().Navigation(p => p.Tags).AutoInclude();
            modelBuilder.Entity<Post>().Navigation(p => p.Likes).AutoInclude();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added && entry.Entity is AuditableEntity addible)
                {
                    addible.DateTime = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified && entry.Entity is Post postEntity)
                {
                    var property = this.Entry(postEntity).Properties.First(p => p.Metadata.Name == nameof(Post.IsDraft));
                    var originalValue = (bool?)(property.OriginalValue);
                    var currentValue = (bool?)(property.CurrentValue);

                    if (originalValue != null && currentValue != null
                        && originalValue != currentValue && !postEntity.IsDraft)
                    {
                        postEntity.DateTime = DateTime.Now;
                    }

                }
            }
            return base.SaveChangesAsync();
        }
    }

}
