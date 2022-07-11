
namespace MiniBlog.Models;

public partial class EFPostsRepo : IPostsRepo
{

    //POSTS
    private MiniBlogDbContext context;

    private ILogger<EFPostsRepo> logger;

    public EFPostsRepo(MiniBlogDbContext injectedContext, ILogger<EFPostsRepo> _logger)
    {
        context = injectedContext;
        logger = _logger;
    }

    public async Task<long?> CreatePost(Post post)
    {
        await context.Posts.AddAsync(post);
        int changes = await context.SaveChangesAsync();
        logger.LogDebug($"Database changes(post adding) : {changes.ToString()}.  Post id: {post.PostId}");
        if (changes > 0) return post.PostId;
        return null;
    }

    public async Task DeletePost(long id)
    {
        Post? post = await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).Where(p => p.PostId == id).FirstOrDefaultAsync();
        if (post is not null)
        {
            context.Posts.Remove(post);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts()
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).OrderByDescending(p => p.DateTime).ToListAsync();
    }

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts(int skip, int take)
    {
        return await context.Posts.Include(p => p.Commentaries).Include(p => p.Tags).OrderByDescending(p => p.DateTime).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<IEnumerable<Post>> RetrieveMultiplePosts(string? tagName, int skip = 0, int take = int.MaxValue)
    {
        if (tagName == null) return Enumerable.Empty<Post>();
        Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName.ToLower());
        if (tag is null) return Enumerable.Empty<Post>();
        return await context.Posts.Include(p => p.Tags).Where(p => p.Tags.Contains(tag)).OrderByDescending(p => p.DateTime).Skip(skip).Take(take).ToListAsync();
    }

    public async Task<Post?> RetrievePost(long id)
    {
        return await context.Posts.Include(p => p.Commentaries.OrderByDescending(c => c.DateTime)).Include(p => p.Tags).Where(p => p.PostId == id).FirstOrDefaultAsync();
    }

     public async Task<Post?> RetrievePost(long id, int commentsSkip, int commentsTake)
    {
        Post? post = await context.Posts.Include(p => p.Tags).Where(p => p.PostId == id).FirstOrDefaultAsync();
        if(post is null) return null;
        post.Commentaries = await context.Commentaries.Where(c => post.Commentaries.Contains(c)).OrderByDescending(c => c.DateTime).Skip(commentsSkip).Take(commentsTake).ToListAsync();
        return post;
    }

    public async Task UpdatePost(Post post)
    {
        Post? existing = await context.Posts.Include(t => t.Tags).FirstOrDefaultAsync(p => p.PostId == post.PostId);

        if (existing is not null)
        {
            context.Entry(existing).CurrentValues.SetValues(post);
            existing.Tags = post.Tags;
            await context.SaveChangesAsync();
        }

    }

    public async Task<int> GetPostsCount()
    {
        return await context.Posts.CountAsync();
    }

    public async Task<int> GetPostsCount(string? tagName)
    {
        Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
        if (tag is null) return 0;
        return await context.Posts.Where(p => p.Tags.Contains(tag)).CountAsync();
    }

    public async Task AddComment(Commentary? commentary, long postId)
    {
        Post? post = await context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
        if (post != null && commentary != null)
        {
            post.Commentaries.Add(commentary);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Tag?> CreateOrRetrieveTag(string tagName)
    {
        tagName = tagName.ToLower().Trim();
        Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);

        if (tag is not null)
        {
            logger.LogDebug($"Tag {tagName} already exists");
            return tag;
        }
        logger.LogDebug($"Tag {tagName} does not exist yet. Creating...");
        tag = new Tag { Name = tagName };
        await context.Tags.AddAsync(tag);
        int changes = await context.SaveChangesAsync();
        if (changes > 0) return tag;
        else return null;
    }

    public async Task<int> GetCommentsCount(long postId)
    {
        Post? post = await context.Posts.Include(p => p.Commentaries).FirstOrDefaultAsync(p => p.PostId == postId);
        if(post == null) return 0;
        return await context.Commentaries.Where(c => post.Commentaries.Contains(c)).CountAsync();
    }

    public async Task DeleteComment(long commId)
    {
        Commentary? comment = await context.Commentaries.FirstOrDefaultAsync(c => c.CommentaryId == commId);
        if(comment != null)
        {
            context.Remove(comment);
            await context.SaveChangesAsync();
        }
    }
}