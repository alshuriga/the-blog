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

    public Task<bool> AnyAsync(ISpecification<Tag>? specification = null)
    {
        if (specification != null)
            return _db.Tags.WithSpecification(specification).AnyAsync();

        return _db.Tags.AnyAsync();
    }

    public async Task<int> CountAsync(ISpecification<Tag>? specification = null)
    {
        if (specification != null)
            return await _db.Tags.WithSpecification(specification).CountAsync();

        return await _db.Tags.CountAsync();
    }

    public Task<IEnumerable<Tag>> ListAsync(ISpecification<Tag>? specification = null)
    {
        IEnumerable<Tag> Tags;
        if (specification != null)
            Tags = _db.Tags.WithSpecification(specification).AsEnumerable();
        else Tags = _db.Tags.AsEnumerable();

        return Task.FromResult(Tags);
    }

    public async Task<Tag?> RetrieveByIdAsync(long id, bool eager = false)
    {
        var query = eager ? _db.Tags.Include(p => p.Posts) : _db.Tags.AsQueryable();
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }
}
