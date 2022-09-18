namespace Blog.Application.Features.Commentaries;

public class CreateCommentaryDTO
{
    public string Text { get; set; } = null!;
    public long PostId { get; set; }
}

