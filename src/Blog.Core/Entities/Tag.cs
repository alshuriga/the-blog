using Blog.Core.Entities.Common;

namespace Blog.Core.Entities;
public class Tag : BaseEntity
{
    public string Name { get; set; } = null!; 
    public ICollection<Post> Posts { get; set; } = new List<Post>();    
}
