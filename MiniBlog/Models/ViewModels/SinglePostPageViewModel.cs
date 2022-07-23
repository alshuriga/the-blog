namespace MiniBlog.Models;

public class SinglePostPageViewModel
{
    public Post Post { get; set; } = null!;
    public PaginationData? CommentsPaginationData { get; set; }
    public int CommentsCount { get; set; }

    public Commentary Commentary { get; set; } = new();
}