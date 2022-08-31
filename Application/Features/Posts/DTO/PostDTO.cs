using Blog.Application.Features.Commentary;
using Blog.Application.Features.Posts.DTO.Common;
using Blog.Application.Features.Tag.DTO;

namespace Blog.Application.Features.Posts.DTO;

public class PostDTO 
{
    public string Text { get; set; } = null!;
    public string Header { get; set; } = null!;
    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
    public DateTime DateTime { get; set; }
    public List<CommentaryDTO> Commentaries { get; set; } = new List<CommentaryDTO>();
}
