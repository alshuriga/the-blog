using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
namespace MiniBlog.Web.ViewModels;


public class PostEditViewModel
{
    public PostDto Post { get; set; } = new();

    [RegularExpression(@"^[a-z][a-z0-9_\s,]+[a-z0-9]$", ErrorMessage = "Please use lowercase tags separated by commas, e.g., \"red, green, blue\"")]
    public string? TagString { get; set; } = String.Empty;
}