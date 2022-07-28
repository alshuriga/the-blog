using Microsoft.EntityFrameworkCore;
using MiniBlog.Core.Entities;

namespace MiniBlog.Infrastructure.Data;

public class MiniBlogDbContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Commentary> Commentaries => Set<Commentary>();

    public MiniBlogDbContext(DbContextOptions<MiniBlogDbContext> options) : base(options)
    {
        
    }
}