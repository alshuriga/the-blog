using Blog.Core.Entities.Common;

namespace Blog.Core.Entities;

public class Post : AuditableEntity
{
    public string Header { get; set; } = null!;
    public string Text { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
    public ICollection<Like> Likes { get; set; } = new List<Like>();
    public bool IsDraft { get; set; }
}
