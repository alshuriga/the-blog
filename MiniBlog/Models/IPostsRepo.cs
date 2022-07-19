namespace MiniBlog.Models;

public interface IPostsRepo
{
    //POSTS
    Task<IEnumerable<Post>> RetrieveMultiplePosts();

    Task<IEnumerable<Post>> RetrieveMultiplePosts(int skip, int take);

    Task<IEnumerable<Post>> RetrieveMultiplePosts(string? tagName, int skip = 0, int take = int.MaxValue);

    Task<Post?> RetrievePost(long id);

    Task<Post?> RetrievePost(long id, int commentsSkip, int commentsTake);

    Task<long?> CreatePost(Post post);

    Task AddComment(Commentary commentary, long postId);

    Task DeleteComment(long commId);

    Task DeletePost(long id);

    Task UpdatePost(Post post);

    Task<int> GetPostsCount();

    Task<int> GetPostsCount(string? tagName);

    Task<int> GetCommentsCount(long postId);

    Task<Tag?> CreateOrRetrieveTag(string TagName);
}