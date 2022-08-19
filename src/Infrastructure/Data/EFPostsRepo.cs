using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Infrastructure.Data;

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

    public Task DeleteAsync(Post post)
    {
        _db.Posts.Remove(post);
        return Task.CompletedTask;
    }

    public async Task DeleteRangeAsync(IEnumerable<Post> posts)
    {
        _db.Posts.RemoveRange(posts);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Post post)
    {
        _db.Posts.Update(post);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(IEnumerable<Post> posts)
    {
        _db.Posts.UpdateRange(posts);
        await _db.SaveChangesAsync();
    }
}