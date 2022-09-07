using Blog.Application.Features.Posts.DTO.Common;
using Blog.Application.Features.Tags.DTO;

namespace Blog.Application.Features.Posts.DTO;

public class CreatePostDTO : IPostDTO
{
    public string Text { get; set; } = null!;
    public string Header { get; set; } = null!;
    public string TagString { get; set; } = null!;
}
