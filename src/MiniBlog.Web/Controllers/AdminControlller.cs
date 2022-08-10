using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        var allUsers = _userManager.Users
            .Select(u => new UserViewModel()
            {
                Id = u.Id,
                Username = u.UserName,
                Email = u.Email,
                Roles = _userManager.GetRolesAsync(u).Result
            }).ToList();
        var model = new UserListsViewModel()
        {
            BasicUsers = allUsers.Where(u => !u.Roles.Any()),
            AdminUsers = allUsers.Where(u => u.Roles.Contains("Admins"))
        };
        return View(model);
    }

    [HttpPost("users/delete")]
    public async Task<IActionResult> DeleteUser([FromForm]string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(UserList));
    }

    [HttpPost]
    public async Task<IActionResult> SwitchAdmin([FromForm]string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        string roleName = "Admins";
        if (user != null)
        {
            IdentityResult res;
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                if ((await _userManager.GetUsersInRoleAsync(roleName)).Count() == 1)
                {
                    ModelState.AddModelError("", "There has to be at least one user with Admin rights");
                    return RedirectToAction(nameof(UserList));
                }
                res = await _userManager.RemoveFromRoleAsync(user, roleName);
            }
            else
            {
                res = await _userManager.AddToRoleAsync(user, roleName);
            }
            if (res.Succeeded) return RedirectToAction(nameof(UserList));
            foreach (var err in res.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return RedirectToAction(nameof(UserList));
        }
        ModelState.AddModelError("", "User not found");
        return RedirectToAction(nameof(UserList));
    }
}