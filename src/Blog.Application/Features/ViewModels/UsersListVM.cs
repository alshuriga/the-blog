using Blog.Application.Features.User.DTO;

namespace Blog.Application.Features.ViewModels
{
    public class UsersListVM
    {
        public List<UserListDTO> NormalUsers { get; set; } = new();
        public List<UserListDTO> AdminUsers { get; set; } = new();
    }
}
