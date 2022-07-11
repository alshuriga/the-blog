namespace MiniBlog.Models;

public class CommentaryPartialViewModel
{
    public Commentary Commentary { get; set; } = null!;
    public long postId { get; set; }
}