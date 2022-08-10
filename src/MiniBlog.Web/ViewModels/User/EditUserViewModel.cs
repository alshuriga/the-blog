using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MiniBlog.Web.ViewModels;

public class EditUserViewModel : UserViewModel
{
    [Compare("Password", ErrorMessage = "Password confirmation does not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public UserEditMode Mode { get; set; } = UserEditMode.Edit;
}

public enum UserEditMode
{
    Create, Edit
}