namespace MiniBlog.Models;

public interface IPostsRepo
{
    //POSTS
    Task<IEnumerable<Post>> RetrieveMultiplePosts(PaginateParams paginateParams, string? tagName = null);

    Task<Post?> RetrievePost(long postId, PaginateParams paginateParams);

    Task<long?> CreatePost(Post post);

    Task AddComment(Commentary commentary, long postId);

    Task DeleteComment(long commId);

    Task DeletePost(long id);

    Task UpdatePost(Post post);

    Task<int> GetPostsCount(string? tagName = null);

    Task<int> GetCommentsCount(long postId);

    Task<Tag?> CreateOrRetrieveTag(string TagName);
}