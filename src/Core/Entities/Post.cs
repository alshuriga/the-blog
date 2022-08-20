using System.ComponentModel.DataAnnotations;

namespace MiniBlog.Core.Entities;

public class Post : BaseEntity
{
  //  public long PostId { get; set; }

    [MaxLength(50, ErrorMessage = "Maximum text length is 50 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Header field cannot be empty")]
    public string Header { get; set; } = null!;

    [MaxLength(1000, ErrorMessage = "Maximum text length is 1000 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text field cannot be empty")]
    public string Text { get; set; } = null!;

    public DateTime DateTime { get; set; } = DateTime.Now;

    public bool IsDraft { get; set; }

    public ICollection<Commentary> Commentaries { get; set; } = null!;

    public ICollection<Tag> Tags { get; set; } = null!;


}