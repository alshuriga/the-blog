using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace MiniBlog.Models;

public class Commentary
{
    [BindNever]
    public long CommentaryId { get; set; }

    [MinLength(1)]
    [MaxLength(50)]
    [Required(ErrorMessage = "Please specify a username")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "Please specify an email address")]
    public string Email { get; set; } = null!;

    [MaxLength(250, ErrorMessage = "Maximum message length is 250 characters")]
    [Required(ErrorMessage = "Text field cannot be empty")]
    public string Text { get; set; } = null!;

    [BindNever]
    public DateTime DateTime { get; set; } = DateTime.Now;
}