using Blog.Application.Features.Posts.DTO.Common;

namespace Blog.Application.Features.Posts.DTO;

public class UpdatePostDTO : IPostDTO
{
    public long PostId { get; set; }
    public string Text { get; set; } = null!;
    public string Header { get; set; } = null!;
    public string TagString { get; set; } = null!;
    public bool IsDraft { get; set; }
}
