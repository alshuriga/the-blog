using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;

namespace MiniBlog.Infrastructure.Data;

public class EfCommentariesRepo : IRepository<Commentary>
{
    private readonly MiniBlogEfContext _db;
    public EfCommentariesRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Commentary entity)
    {
        await _db.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Commentary> entities)
    {
        await _db.AddRangeAsync(entities);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Commentary entity)
    {
        _db.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<Commentary> entities)
    {
        _db.RemoveRange(entities);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Commentary entity)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Commentary> entities)
    {
        _db.UpdateRange(entities);
        await _db.SaveChangesAsync();
    }
}