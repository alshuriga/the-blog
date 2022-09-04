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

    public async Task<User> GetUserByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return new User() { Username = user.UserName, Email = user.Email };
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
        var user = new IdentityUser() { UserName = username, Email = email };
        await _userManager.CreateAsync(user);
        await _userManager.AddPasswordAsync(user, password);
    }
}
