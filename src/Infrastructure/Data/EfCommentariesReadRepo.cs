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

    public Task<bool> AnyAsync(ISpecification<Commentary> specification)
    {
        return Task.FromResult(_db.Commentaries.WithSpecification(specification).Any());
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(_db.Commentaries.Count());
    }

    public Task<int> CountAsync(ISpecification<Commentary> specification)
    {
        return Task.FromResult(_db.Commentaries.WithSpecification(specification).Count());
    }

    public Task<IEnumerable<Commentary>> ListAsync()
    {
        return  Task.FromResult(_db.Commentaries.AsEnumerable());
    }

    public Task<IEnumerable<Commentary>> ListAsync(ISpecification<Commentary> specification)
    {
        return  Task.FromResult(_db.Commentaries.WithSpecification(specification).AsEnumerable());
    }

    public async Task<Commentary?> RetrieveByIdAsync(long id, bool eager = false)
    {
        var query = _db.Commentaries.AsQueryable();
        if(eager) query.Include(c => c.Post).ThenInclude(p => p!.Tags);
        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }
}