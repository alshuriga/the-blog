using Microsoft.AspNetCore.Identity;

namespace MiniBlog.Web.ViewModels;

public static class ViewModelFactory
{
    public static EditUserViewModel CreateSignUpUserModel()
    {
        return new EditUserViewModel()
        {
            Mode = UserEditMode.Create
        };
    }

    public static EditUserViewModel CreateEditUserModel(IdentityUser user)
    {
        return new EditUserViewModel()
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            Mode = UserEditMode.Edit
        };
    }
} 