using Blog.Application.Interfaces;
using Blog.Application.Models;
using Microsoft.AspNetCore.Identity;

namespace Blog.Infrastructure.Identity;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;


    public UserService(UserManager<IdentityUser> userManager,  SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<User> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        return new User() { Id = user.Id, Username = user.UserName, Email = user.Email, Roles = roles };
    }

    public async Task<User> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        var roles = (await _userManager.GetRolesAsync(user)).ToList();
        return new User() {Id = user.Id, Username = user.UserName, Email = user.Email, Roles = roles };
    }

    public async Task SignInAsync(string username, string password)
    {
        var res = await _signInManager.PasswordSignInAsync(username, password, false, false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task SignUpAsync(string username, string email, string password)
    {
        var user = new IdentityUser() {UserName = username, Email = email };
        await _userManager.CreateAsync(user, password);
    }



    public async Task AddToRoleAsync(string id, string roleName)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (!await _userManager.IsInRoleAsync(user, roleName))
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }
    }

    public async Task RemoveFromRoleAsync(string id, string rolename)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (await _userManager.IsInRoleAsync(user, rolename))
        {
            await _userManager.RemoveFromRoleAsync(user, rolename);
        }
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if(user != null)
            await _userManager.DeleteAsync(user);
    }

    public async Task<List<User>> ListUsersAsync(string? rolename = null)
    {
        IEnumerable<IdentityUser> users;
        if(rolename == null)
        {
            users = _userManager.Users.ToList();
        }
       else
        {
            users = (await _userManager.GetUsersInRoleAsync(rolename));
        }
        return users.Select(u => new User() { Id = u.Id, Username = u.UserName, Email = u.Email }).ToList();
    }

    public async Task<List<User>> ListNoRoleUsersAsync()
    {

        var users = _userManager.Users.ToList();
        List<IdentityUser> noRoleUsers = new();
        foreach(var user in users)
        {
            if(!(await _userManager.GetRolesAsync(user)).Any())
            {
                noRoleUsers.Add(user);
            }
        }
        return noRoleUsers.Select(u => new User() { Id = u.Id, Username = u.UserName, Email = u.Email }).ToList();
    }


}
