using Blog.Application.Features.Tags.DTO;

namespace Blog.Application.Features.Posts.DTO;

public class PostDTO
{
    public long Id { get; set; }
    public string? Text { get; set; }
    public string? Header { get; set; }
    public List<TagDTO> Tags { get; set; } = new List<TagDTO>();
    public DateTime DateTime { get; set; }
    public bool IsDraft { get; set; }
}
