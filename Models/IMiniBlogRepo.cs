namespace MiniBlog.Models;

public interface IMiniBlogRepo
{
    Task<IEnumerable<Post>> RetrieveAllPosts();

    Task<Post?> RetrievePost(long id);

    Task CreatePost(Post post);

    Task DeletePost(long id);

    Task UpdatePost(Post post);
}