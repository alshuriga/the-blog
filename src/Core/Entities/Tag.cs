

namespace MiniBlog.Core.Entities;

public class Tag
{
    public int TagId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Post>? Posts { get; set; } 
}