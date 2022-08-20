using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Web.ViewModels;


namespace MiniBlog.Web.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet()]
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
    public IActionResult SignUp(string? returnUrl)
    {
        ViewData["returnUrl"] = returnUrl ?? "/";
        return View(ViewModelFactory.CreateSignUpUserModel());
    }

    [HttpPost]
    public async Task<IActionResult> SignUp(EditUserViewModel user, string? returnUrl)
    {
        if (ModelState.IsValid)
        {
            IdentityUser idUser = new IdentityUser { UserName = user.Username, Email = user.Email };
            var result = await _userManager.CreateAsync(idUser, user.Password);
            if (result.Succeeded)
            {
                return Redirect(returnUrl ?? "/");
            }
            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
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

    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    [HttpGet("{username}")]
    public async Task<IActionResult> EditUser(string userName)
    {
        IdentityUser? user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            return View(ViewModelFactory.CreateEditUserModel(user));
        }
        return BadRequest();
    }
    
}