using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Models;

namespace MiniBlog.Components;

public class AddComment : ViewComponent
{
    private readonly IPostsRepo repo;
    
    public AddComment(IPostsRepo _repo)
    {
        repo = _repo;
    }

    public IViewComponentResult Invoke(long postId)
    {
        ViewBag.PostId = postId;
        var CommentModel = new CommentaryViewModel();
        return View(CommentModel);
    }
}