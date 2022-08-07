
using System.Net;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Web.ViewModels;

namespace MiniBlog.Web.Components;

public class CommentaryForm : ViewComponent
{
    private readonly UserManager<IdentityUser> _userManager;
    

    public CommentaryForm(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(long postId)
    {
        CommentaryViewModel? model = null;
        if (User.Identity?.IsAuthenticated != null && User.Identity.IsAuthenticated)
        {
            // var mgr = HttpContext.RequestServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            model = new CommentaryViewModel { Username = user.UserName, Email = user.Email };
        }

        ViewData["postId"] = postId;
        return View("Default", model);
    }
}