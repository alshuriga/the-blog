using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using Ardalis.Specification;

namespace MiniBlog.Infrastructure.Data;

public class EfCommentariesReadRepo : IReadRepository<Commentary>
{
    private readonly MiniBlogEfContext _db;

    public EfCommentariesReadRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public Task<bool> AnyAsync(ISpecification<Commentary>? specification = null)
    {
        if (specification != null)
            return _db.Commentaries.WithSpecification(specification).AnyAsync();

        return _db.Commentaries.AnyAsync();
    }

    public async Task<int> CountAsync(ISpecification<Commentary>? specification = null)
    {
        if (specification != null)
            return await _db.Commentaries.WithSpecification(specification).CountAsync();

        return await _db.Commentaries.CountAsync();
    }

    public Task<IEnumerable<Commentary>> ListAsync(ISpecification<Commentary>? specification = null)
    {
        IEnumerable<Commentary> Commentaries;
        if (specification != null)
            Commentaries = _db.Commentaries.WithSpecification(specification).AsEnumerable();
        else Commentaries = _db.Commentaries.AsEnumerable();

        return Task.FromResult(Commentaries);
    }

    public async Task<Commentary?> RetrieveByIdAsync(long id, bool eager = false)
    {
        var query = eager ? _db.Commentaries.Include(p => p.Post) : _db.Commentaries.AsQueryable();
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }
}