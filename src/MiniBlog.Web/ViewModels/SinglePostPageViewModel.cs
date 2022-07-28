using MiniBlog.Core.Models;

namespace MiniBlog.Web.ViewModels;

public class SinglePostPageViewModel
{
    public Post Post { get; set; } = null!;
    public PaginationData? CommentsPaginationData { get; set; }
    public int CommentsCount { get; set; }

}