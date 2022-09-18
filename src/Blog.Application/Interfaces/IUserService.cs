using Blog.Application.Models;

namespace Blog.Application.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByNameAsync(string userName);
        Task<bool> SignInAsync(string username, string password);
        Task SignOutAsync();
        Task SignUpAsync(string username, string email, string password);
        Task DeleteUserAsync(string Id);
        Task AddToRoleAsync(string username, string rolename);
        Task RemoveFromRoleAsync(string Id, string rolename);
        Task<List<User>> ListUsersAsync(string? rolename = null);
        Task<List<User>> ListNoRoleUsersAsync();
    }
}
