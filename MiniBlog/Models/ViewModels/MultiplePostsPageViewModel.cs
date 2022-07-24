namespace MiniBlog.Models;

public class MultiplePostsPageViewModel 
{
    public IEnumerable<Post> Posts {get; set; } = Enumerable.Empty<Post>();
    public PaginationData? PaginationData { get; set; }
    public int? PostsCount { get; set; }
    public string? TagName { get; set; }
    public string? Url { get; set; }
}