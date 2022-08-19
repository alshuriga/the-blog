using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MiniBlog.Web.Pages;


[Authorize(Roles = "Admins")]
public class DeletePost : PageModel
{
    private readonly IUnitOfWork _unit;

    public DeletePost(IUnitOfWork unit)
    {
        _unit = unit;
    }

    public async Task<IActionResult> OnPost(long postId)
    {
        await _unit.postRepo.DeleteAsync(new Post { Id = postId });
        return RedirectToPage("List");
    }
}