namespace MiniBlog.Models;

public class PostsPageViewModel 
{
    public PostsPageViewModel(IEnumerable<Post> _posts, PaginationData _paginationData, string? _tagName = null)
    {
        Posts = _posts;
        PaginationData = _paginationData;
        TagName = _tagName;
    }
    public IEnumerable<Post> Posts {get; set; }
    public PaginationData PaginationData { get; set; }
    public string? TagName { get; set; }
}