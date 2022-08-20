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

    public async Task AddRangeAsync(IEnumerable<Post> posts)
    {
        await _db.Posts.AddRangeAsync(posts);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Post post)
    {
        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(IEnumerable<Post> posts)
    {
        _db.Posts.RemoveRange(posts);
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

    public async Task UpdateRangeAsync(IEnumerable<Post> posts)
    {
        _db.Posts.UpdateRange(posts);
        await _db.SaveChangesAsync();
    }
}