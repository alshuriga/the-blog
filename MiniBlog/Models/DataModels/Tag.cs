using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MiniBlog.Models;

public class Tag
{
    public int TagId { get; set; }

    public string Name { get; set; } = null!;

    public ICollection<Post>? Posts { get; set; } 
}