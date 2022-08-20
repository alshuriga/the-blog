using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;

namespace MiniBlog.Infrastructure.Data;

public class EfTagsRepo : IRepository<Tag>
{
    private readonly MiniBlogEfContext _db;
    public EfTagsRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Tag entity)
    {
        await _db.Tags.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<Tag> entities)
    {
        await _db.Tags.AddRangeAsync(entities);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Tag entity)
    {
        _db.Tags.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<Tag> entities)
    {
        _db.Tags.RemoveRange(entities);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Tag entity)
    {
        _db.Tags.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Tag> entities)
    {
        _db.Tags.UpdateRange(entities);
        await _db.SaveChangesAsync();
    }
}