using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Web.ViewModels;

namespace MiniBlog.Web.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        if(returnUrl != null) ViewBag.returnUrl = returnUrl;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login([FromForm] UserViewModel model, [FromQuery] string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            IdentityUser? user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var res = await  _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (res.Succeeded)
                {
                    Console.Write("\n\nlog in success\n\n");
                    if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "Username or/and password is incorrect");
            }
        }

        return View();
    }

    [HttpGet]
    public async Task<IActionResult> LogOut(string? returnUrl)
    {
        await _signInManager.SignOutAsync();
        if (string.IsNullOrEmpty(returnUrl)) return RedirectToAction("Index", "Home");
        return Redirect(returnUrl);
    }

    [HttpGet]
    public IActionResult AccessDenied(string? returnUrl)
    {
        return View(model: returnUrl);
    }


}