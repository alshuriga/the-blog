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

    public async Task DeleteAsync(Commentary entity)
    {
        _db.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Commentary entity)
    {
        _db.Update(entity);
        await _db.SaveChangesAsync();
    }

}