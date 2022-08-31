using Blog.Core.Entities.Common;

namespace Blog.Core.Entities;

public class Post : BaseEntity
{
    public string Header { get; set; } = null!;
    public string Text { get; set; } = null!;
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    public DateTime DateTime { get; set; }
    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();
}
