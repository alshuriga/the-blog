
namespace MiniBlog.Models;

public class EFMiniBlogRepo : IMiniBlogRepo
{
    private MiniBlogDbContext context;

    public EFMiniBlogRepo(MiniBlogDbContext injectedContext)
    {
        context = injectedContext;
    }

    public async Task CreatePost(Post post)
    {
        await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
    }

    public async Task DeletePost(long id)
    {
        Post? post = await context.Posts.Where(p => p.PostId == id).FirstOrDefaultAsync();
        if (post is not null)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Post>> RetrieveAllPosts()
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).OrderBy(p => p.DateTime).ToListAsync();
    }

    public async Task<Post?> RetrievePost(long id)
    {
        return await context.Posts.Where(p => p.PostId == id).FirstOrDefaultAsync();
    }

    public async Task UpdatePost(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }
}