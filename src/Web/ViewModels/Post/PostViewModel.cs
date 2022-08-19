using MiniBlog.Core.Models;

namespace MiniBlog.Web.ViewModels;

public class PostViewModel
{
    public PostPartialViewModel? PostPartial { get; set; }
    public PaginationData? CommentsPaginationData { get; set; }
    public IEnumerable<CommentaryDto> Commentaries { get; set; } = Enumerable.Empty<CommentaryDto>();

}