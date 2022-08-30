using MiniBlog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace MiniBlog.Infrastructure.Data;

public class EfRepository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly MiniBlogEfContext _db;

    public EfRepository(MiniBlogEfContext db)
    {
        _db = db;
    }

    public async Task AddAsync(T entity)
    {
        _db.Add(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _db.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _db.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        await _db.SaveChangesAsync();
    }
}
