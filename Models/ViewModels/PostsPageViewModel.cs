namespace MiniBlog.Models;

public class PostsPageViewModel 
{
    public IEnumerable<Post> Posts {get; set; } = Enumerable.Empty<Post>();
    public PaginationData? PaginationData { get; set; }
    public string? TagName { get; set; }
    public string? Url { get; set; }
}