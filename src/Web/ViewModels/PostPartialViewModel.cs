namespace MiniBlog.Web.ViewModels;

public class PostPartialViewModel
{
    public PostDto Post { get; set; } = null!;
    public IEnumerable<string> TagNames { get; set; } = Enumerable.Empty<string>();
    public bool CommentsButton { get; set; }
    public bool IsDraft { get; set; }
    public int CommentariesCount { get; set; }
}