using MiniBlog.Core.Models;

namespace MiniBlog.Web.ViewModels;

public class PostsIndexViewModel 
{
    public IEnumerable<PostPartialViewModel> Posts {get; set; } = Enumerable.Empty<PostPartialViewModel>();
    public PaginationData? PaginationData { get; set; }
    public int? PostsCount { get; set; }
    public string? TagName { get; set; }
    public string? Url { get; set; }
}