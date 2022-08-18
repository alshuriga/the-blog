using Microsoft.EntityFrameworkCore;
using MiniBlog.Core.Entities;

namespace MiniBlog.Infrastructure.Data;

public class MiniBlogEfContext : DbContext
{
    public DbSet<Post> Posts => Set<Post>();

    public DbSet<Tag> Tags => Set<Tag>();

    public DbSet<Commentary> Commentaries => Set<Commentary>();

    public MiniBlogEfContext(DbContextOptions<MiniBlogEfContext> options) : base(options)
    {
        
    }
}