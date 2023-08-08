namespace Blog.Application.Features.Posts.DTO.Common;

public abstract class WritablePostDTO
{
    public string? Text { get; set; }
    public string? Header { get; set; }
    public string? TagString { get; set; }
    public bool IsDraft { get; set; }
}
