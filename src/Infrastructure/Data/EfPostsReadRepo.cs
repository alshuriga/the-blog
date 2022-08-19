using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

public class EfPostsReadRepo : IReadRepository<Post>
{
    private readonly MiniBlogEfContext _db;
    public EfPostsReadRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public Task<bool> AnyAsync(ISpecification<Post> specification)
    {
        return Task.FromResult(_db.Posts.WithSpecification(specification).Any());
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(_db.Posts.Count());
    }

    public Task<int> CountAsync(ISpecification<Post> specification)
    {
        return Task.FromResult(_db.Posts.WithSpecification(specification).Count());
    }

    public  Task<IEnumerable<Post>> ListAsync()
    {
        return Task.FromResult(_db.Posts.AsEnumerable());
    }

    public  Task<IEnumerable<Post>> ListAsync(ISpecification<Post> specification)
    {
        return Task.FromResult(_db.Posts.WithSpecification(specification).AsEnumerable());
    }
}
