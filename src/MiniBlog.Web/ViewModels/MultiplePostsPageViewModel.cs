using MiniBlog.Core.Models;

namespace MiniBlog.Web.ViewModels;

public class MultiplePostsPageViewModel 
{
    public IEnumerable<Post> Posts {get; set; } = Enumerable.Empty<Post>();
    public PaginationData? PaginationData { get; set; }
    public int? PostsCount { get; set; }
    public string? TagName { get; set; }
    public string? Url { get; set; }
}