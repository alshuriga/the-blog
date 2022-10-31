using Blog.Application.Features.Posts.DTO.Common;

namespace Blog.Application.Features.Posts.DTO;

public class UpdatePostDTO : IPostDTO
{
    public long Id { get; set; }
    public string? Text { get; set; }
    public string? Header { get; set; }
    public string? TagString { get; set; }
    public bool IsDraft { get; set; }
}
