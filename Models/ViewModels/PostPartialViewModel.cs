namespace MiniBlog.Models;

public class PostPartialViewModel
{
    public Post Post { get; set; } = null!;
    public int CommentCount { get; set;}
    public bool CommentsButton { get; set; }
}