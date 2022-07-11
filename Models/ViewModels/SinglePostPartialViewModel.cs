namespace MiniBlog.Models;

public class SinglePostPartialViewModel
{
    public Post Post { get; set; } = null!;
    public int CommentCount { get; set;}
    public bool CommentsButton { get; set; }
}