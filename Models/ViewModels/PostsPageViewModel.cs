namespace MiniBlog.Models;

public class PostsPageViewModel 
{
    public PostsPageViewModel(IEnumerable<Post> _posts, int _currentPage, int _pageNumber, string? _tagName = null)
    {
        Posts = _posts;
        CurrentPage = _currentPage;
        PageNumber = _pageNumber;
        TagName = _tagName;
    }
    public IEnumerable<Post> Posts {get; set; }
    public int CurrentPage { get; set; }
    public int PageNumber { get; set; }
    public string? TagName { get; set; }
}