using Blog.Core.Entities.Common;

namespace Blog.Core.Entities;

public class Commentary : AddibleEntity
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Text { get; set; } = null!;
    public long PostId { get; set; }
    public Post? Post { get; set; }
}
