
namespace MiniBlog.Models;

public partial class EFPostsRepo : IPostsRepo
{

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


    public async Task<IEnumerable<Post>> RetrieveMultiplePosts(PaginateParams paginateParams, string? tagName = null)
    {
        IQueryable<Post> query = context.Posts.Include(p => p.Tags).OrderByDescending(p => p.DateTime);
        if (tagName != null)
        {
            tagName = tagName.Trim().ToLower();
            Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName);
            if (tag is null) return Enumerable.Empty<Post>();
            query = query.Where(p => p.Tags.Contains(tag));
        }
        return await query.Skip(paginateParams.Skip).Take(paginateParams.Take).ToListAsync();
    }

    public async Task<Post?> RetrievePost(long postId, PaginateParams postParams)
    {
        Post? post = await context.Posts.Include(p => p.Tags).Where(p => p.PostId == postId).FirstOrDefaultAsync();
        if (post is null) return null;
        post.Commentaries = await context.Commentaries.Where(c => post.Commentaries.Contains(c)).OrderByDescending(c => c.DateTime).Skip(postParams.Skip).Take(postParams.Take).ToListAsync();
        return post;
    }

    public async Task UpdatePost(Post post)
    {
        Post? existing = await context.Posts.Include(t => t.Tags).FirstAsync(p => p.PostId == post.PostId);
        context.Entry(existing).CurrentValues.SetValues(post);
        existing.Tags = post.Tags;
        await context.SaveChangesAsync();
    }

    public async Task<int> GetPostsCount(string? tagName = null)
    {
        if (tagName != null)
        {
            Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
            if (tag is null) return 0;
            return await context.Posts.Where(p => p.Tags.Contains(tag)).CountAsync();
        }
        return await context.Posts.CountAsync();
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
        if (post == null) return 0;
        return await context.Commentaries.Where(c => post.Commentaries.Contains(c)).CountAsync();
    }

    public async Task DeleteComment(long commId)
    {
        Commentary? comment = await context.Commentaries.FirstOrDefaultAsync(c => c.CommentaryId == commId);
        if (comment != null)
        {
            context.Remove(comment);
            await context.SaveChangesAsync();
        }
    }
}