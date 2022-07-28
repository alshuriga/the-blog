

namespace MiniBlog.Core.Entities;

public class Commentary
{
    public long CommentaryId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime DateTime { get; set; } = DateTime.Now;
    public long PostId { get; set; }
    public Post? Post { get; set; }
}