using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MiniBlog.Web.ViewModels;

public class UserViewModel
{
    [BindNever]
    public string Id { get; set;  } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    public string Password { get; set; } = string.Empty;
}