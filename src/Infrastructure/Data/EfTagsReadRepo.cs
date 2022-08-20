using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;
using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class EfTagsReadRepo : IReadRepository<Tag>
{
    private readonly MiniBlogEfContext _db;
    public EfTagsReadRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public Task<bool> AnyAsync(ISpecification<Tag> specification)
    {
        return Task.FromResult(_db.Tags.WithSpecification(specification).Any());
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(_db.Tags.Count());
    }

    public Task<int> CountAsync(ISpecification<Tag> specification)
    {
        return Task.FromResult(_db.Tags.WithSpecification(specification).Count());
    }

    public  Task<IEnumerable<Tag>> ListAsync()
    {
        return Task.FromResult(_db.Tags.AsEnumerable());
    }

    public  Task<IEnumerable<Tag>> ListAsync(ISpecification<Tag> specification)
    {
        return Task.FromResult(_db.Tags.WithSpecification(specification).AsEnumerable());
    }

    public async Task<Tag?> RetrieveByIdAsync(long id, bool eager = false)
    {
        var query = eager ? _db.Tags.Include(t => t.Posts) : _db.Tags.AsQueryable();
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }
}
