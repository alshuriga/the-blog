using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MiniBlog.Models;

public class Post
{
    public long PostId { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string Header { get; set; } = null!;

    [MaxLength(1000)]
    public string Text { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public bool IsDraft { get; set; }

    public ICollection<Commentary> Commentaries { get; set; } = new List<Commentary>();

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();


}