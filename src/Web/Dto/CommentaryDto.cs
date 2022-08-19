using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MiniBlog.Web.ViewModels;

[BindNever]
public class CommentaryDto
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    [BindRequired]
    [MaxLength(150, ErrorMessage = "Maximum message length is 150 characters")]
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text field cannot be empty")]
    public string Text { get; set; } = string.Empty;

    public DateTime DateTime { get; set; }
}