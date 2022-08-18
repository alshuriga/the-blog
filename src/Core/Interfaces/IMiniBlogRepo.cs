using MiniBlog.Core.Models;
using MiniBlog.Core.Entities;

namespace MiniBlog.Core.Interfaces;

public interface IMiniBlogRepo
{
    Task<IEnumerable<Post>> RetrievePostsRange(PaginateParams paginateParams, string? tagName = null);

    Task<long?> CreatePost(Post post);

    Task<Post?> RetrievePost(long postId, PaginateParams paginateParams);

    Task UpdatePost(Post post);

    Task DeletePost(long id);

    Task CreateComment(Commentary commentary, long postId);

    Task DeleteComment(long commId);

    Task CreateTagIfNotExist(Tag tag);

    Task<Tag?> RetrieveTagByName(string tagName);
    
    Task<int> GetPostsCount(string? tagName = null);
    
    Task <int> GetCommentariesCount(long postId);
}