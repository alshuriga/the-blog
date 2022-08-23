using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class EfPostsReadRepo : IReadRepository<Post>
{
    private readonly MiniBlogEfContext _db;
    public EfPostsReadRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public Task<bool> AnyAsync(ISpecification<Post>? specification = null)
    {
        if (specification != null) 
            return _db.Posts.WithSpecification(specification).AnyAsync();

        return _db.Posts.AnyAsync();
    }

    public async Task<int> CountAsync(ISpecification<Post>? specification = null)
    {
        if (specification != null)
            return await _db.Posts.WithSpecification(specification).CountAsync();

       return await _db.Posts.CountAsync();
    }


    public Task<IEnumerable<Post>> ListAsync(ISpecification<Post>? specification = null)
    {
        IEnumerable<Post> posts;
        if (specification != null)
            posts = _db.Posts.WithSpecification(specification).AsEnumerable();
        else posts = _db.Posts.AsEnumerable();

        return Task.FromResult(posts);
    }

    public async Task<Post?> RetrieveByIdAsync(long id, bool eager = false)
    {
        var query = eager ? _db.Posts.Include(p => p.Tags).Include(p => p.Commentaries) : _db.Posts.AsQueryable();
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }
}
