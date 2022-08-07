using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Web.ViewModels;

namespace MiniBlog.Web.Controllers;

[Authorize(Roles = "Admins")]
[Route("[controller]")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    [HttpGet("users/list")]
    public IActionResult UserList()
    {
        IEnumerable<UserViewModel> users = _userManager.Users
            .Select(u => new UserViewModel()
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result
            });
        
        return View(users);
    }

    [HttpPost("users/delete")]
    public async Task<IActionResult> DeleteUser([FromForm]string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(UserList));
    }

    [HttpGet("users/create")]
    public IActionResult CreateUser()
    {
        return View();
    }
    
    [HttpPost("users/create")]
    public async Task<IActionResult> CreateUser([FromForm] UserViewModel user)
    {
        if (ModelState.IsValid)
        {
            IdentityUser idUser = new IdentityUser { UserName = user.Username, Email = user.Email };
            var result = await _userManager.CreateAsync(idUser, user.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(UserList));
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
        }

        return View();
    }
}