namespace  MiniBlog.Web.ViewModels;

public class UserListsViewModel
{
    public IEnumerable<UserViewModel> BasicUsers { get; set; } = Enumerable.Empty<UserViewModel>();
    public IEnumerable<UserViewModel> AdminUsers { get; set; } = Enumerable.Empty<UserViewModel>();
}

