using Microsoft.AspNetCore.Mvc;


namespace MiniBlog.Controllers;

public class HomeController : Controller
{
    private IMiniBlogRepo repo;

    public HomeController(IMiniBlogRepo injectedRepo)
    {
        repo = injectedRepo;
    }
    public async Task<ViewResult> Index() 
    {
        var posts = await repo.RetrieveAllPosts();
        return View(posts);
    }
}