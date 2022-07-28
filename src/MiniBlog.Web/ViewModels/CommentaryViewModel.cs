using System.ComponentModel.DataAnnotations;

namespace MiniBlog.Web.ViewModels;

public class CommentaryViewModel
{
    [MinLength(5)]
    [MaxLength(30)]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify a username")]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify an email address")]
    [EmailAddress(ErrorMessage = "Incorrect email address")]
    public string Email { get; set; } = string.Empty;

    [MaxLength(150, ErrorMessage = "Maximum message length is 150 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text field cannot be empty")]
    public string Text { get; set; } = string.Empty;
}