using Ardalis.Specification;
using MiniBlog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Ardalis.Specification.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MiniBlog.Infrastructure.Data;

public class EfReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly MiniBlogEfContext _db;
    public EfReadRepository(MiniBlogEfContext db)
    {
        _db = db;   
    }

    public async Task<bool> AnyAsync(ISpecification<T>? specification = null)
    {
        if(specification != null)
            return await _db.Set<T>().WithSpecification(specification).AnyAsync();

        return await _db.Set<T>().AnyAsync();    
    }

    public async Task<int> CountAsync(ISpecification<T>? specification = null)
    {
        if (specification != null)
            return await _db.Set<T>().WithSpecification(specification).CountAsync();

        return await _db.Set<T>().CountAsync();
    }

    public async Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null)
    {
        if (specification != null)
            return await _db.Set<T>().WithSpecification(specification).ToListAsync();

        return await _db.Set<T>().ToListAsync();
    }

    public async Task<T?> RetrieveByIdAsync(long id, Expression<Func<T, object>>[]? includes = null)
    {
        var query = _db.Set<T>().AsQueryable();
        if(includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }
        return await query.FirstOrDefaultAsync(T => T.Id == id);
    }
}
