namespace MiniBlog.Models;

public class PostPartialViewModel
{
    public Post Post { get; set; } = null!;
    public bool CommentsButton { get; set; }
}