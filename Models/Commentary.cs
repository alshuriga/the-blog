using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MiniBlog.Models;

public class Commentary
{
    public long CommentaryId { get; set; }

    public long PostId { get; set; }
    public Post Post { get; set; } = null!;

    [MinLength(1)]
    [MaxLength(50)]
    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    [MaxLength(250, ErrorMessage = "Maximum message length is 250 characters.")]
    public string Text { get; set; } = null!;

    public DateTime DateTime { get; set; }
}