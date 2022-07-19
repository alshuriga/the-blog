using System.ComponentModel.DataAnnotations;

namespace MiniBlog.Models;

public class Tag
{
    public int TagId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Post>? Posts { get; set; } 
}