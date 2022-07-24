
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


    public async Task<IEnumerable<Post>> RetrievePostsRange(PaginateParams paginateParams, string? tagName = null)
    {
        IQueryable<Post> query = context.Posts.Include(p => p.Tags).Include(p => p.Commentaries).OrderByDescending(p => p.DateTime);
        if (tagName != null)
        {
            tagName = tagName.Trim().ToLower();
            Tag? tag = await context.Tags.FirstOrDefaultAsync(t => t.Name.ToLower() == tagName);
            if (tag is null) return Enumerable.Empty<Post>();
            query = query.Where(p => p.Tags.Contains(tag));
        }
        return await query.Skip(paginateParams.Skip).Take(paginateParams.Take).ToListAsync();
    }

    public async Task<long?> CreatePost(Post post)
    {
        await context.Posts.AddAsync(post);
        int changes = await context.SaveChangesAsync();
        logger.LogDebug($"Database changes(post adding) : {changes.ToString()}.  Post id: {post.PostId}");
        if (changes > 0) return post.PostId;
        return null;
    }

    public async Task<Post?> RetrievePost(long postId, PaginateParams commentsParams)
    {
        Post? post = await context.Posts.Include(p => p.Tags).Where(p => p.PostId == postId).FirstOrDefaultAsync();
        if (post is null) return null;
        post.Commentaries = await context.Commentaries.Where(c => c.PostId == postId).OrderByDescending(c => c.DateTime).Skip(commentsParams.Skip).Take(commentsParams.Take).ToListAsync();
        return post;
    }
    
    public async Task UpdatePost(Post post)
    {
        Post? existing = await context.Posts.Include(t => t.Tags).FirstAsync(p => p.PostId == post.PostId);
        context.Entry(existing).CurrentValues.SetValues(post);
        existing.Tags = post.Tags;
        await context.SaveChangesAsync();
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


    public async Task CreateComment(Commentary? commentary, long postId)
    {
        Post? post = await context.Posts.FirstOrDefaultAsync(p => p.PostId == postId);
        if (post != null && commentary != null)
        {
            post.Commentaries.Add(commentary);
            await context.SaveChangesAsync();
        }
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

    public async Task CreateTagIfNotExist(Tag tag)
    {
        if(await RetrieveTagByName(tag.Name) == null)
        {
        await context.Tags.AddAsync(tag);
        await context.SaveChangesAsync();
        }

    }

    public async Task<Tag?> RetrieveTagByName(string tagName)
    {
        Tag? tag = await context.Tags.FirstOrDefaultAsync(t => String.Compare(t.Name, tagName, StringComparison.OrdinalIgnoreCase) == 0);
        return tag;
    }

    public async Task<int> GetPostsCount(string? tagName = null)
    {
        var query = context.Posts.AsQueryable();
        if(tagName is not null) query = query.Where(p => p.Tags.Where(t => t.Name == tagName).Any());
        return await query.CountAsync();
    }

     public async Task<int> GetCommentariesCount(long postId)
    {
        return await context.Commentaries.Where(c => c.PostId == postId).CountAsync();
    }
}