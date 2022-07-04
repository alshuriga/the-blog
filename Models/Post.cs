using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MiniBlog.Models;

public class Post
{
    public long PostId { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    public string Header { get; set; } = null!;

    [MaxLength(250)]
    public string Text { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public bool IsDraft { get; set; }

    public List<Commentary>? Commentaries { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();


}