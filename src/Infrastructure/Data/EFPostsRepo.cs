using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class EfPostsRepo : IRepository<Post>
{
    private readonly MiniBlogEfContext _db;
    public EfPostsRepo(MiniBlogEfContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Post post)
    {
        await _db.Posts.AddAsync(post);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post post)
    {
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        //var existing = _db.Posts.AsNoTracking().Include(p => p.Tags).First(post => post.Id == post.Id);
        //_db.Entry(existing).CurrentValues.SetValues(post);
        //existing.Tags = post.Tags;
        _db.Update(post);
       await _db.SaveChangesAsync();
    }
}