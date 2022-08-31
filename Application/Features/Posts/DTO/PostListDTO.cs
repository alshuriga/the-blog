
using Blog.Application.Features.Tag.DTO;

namespace Blog.Application.Features.Posts.DTO;

public class PostListDTO
{
    public string Text { get; set; } = null!;
    public string Header { get; set; } = null!;
    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
    public DateTime DateTime { get; set; }
    public int CommentariesCount { get; set; }
}
