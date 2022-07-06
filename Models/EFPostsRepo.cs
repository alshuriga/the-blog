
namespace MiniBlog.Models;

public partial class EFPostsRepo : IPostsRepo
{

    //POSTS
    private MiniBlogDbContext context;

    public EFPostsRepo(MiniBlogDbContext injectedContext)
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

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts()
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).OrderBy(p => p.DateTime).ToListAsync();
    }

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts(int skip, int take)
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).OrderBy(p => p.DateTime).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts(string tagName, int skip = 0, int take = int.MaxValue)
    {
        Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
        if(tag is null) return Enumerable.Empty<Post>();
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).Where(p => p.Tags.Contains(tag)).OrderBy(p => p.DateTime).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Post?> RetrievePost(long id)
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).Where(p => p.PostId == id).FirstOrDefaultAsync();
    }

    public async Task UpdatePost(Post post)
    {
        context.Entry(post).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task<int> GetPostsCount()
    {
        return await context.Posts.CountAsync();
    }
 }