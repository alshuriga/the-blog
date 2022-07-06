using Microsoft.AspNetCore.Mvc;


namespace MiniBlog.Controllers;

public class HomeController : Controller
{

    const int perPage = 5;
    private IPostsRepo repo;

    private ILogger logger;

    public HomeController(IPostsRepo _repo, ILogger<HomeController> _logger)
    {
        repo = _repo;
        logger = _logger;
    }

    [HttpGet("/{currentPage:int=1}")]
    public async Task<ViewResult> Index(int currentPage)
    {
        int pageNumber = (await repo.GetPostsCount() / perPage) + 1;
        int skip = currentPage <= 0 ? 0 : (currentPage -1)* perPage;
        logger.LogDebug($"Skip = {skip.ToString()}");
        var posts = await repo.RetrieveMultiplePosts(skip, perPage);
        var model = new PostsPageViewModel(posts, currentPage, pageNumber);
        return View("Index", model);
    }

    [HttpGet("/tag/{tagName:alpha}/{currentPage:int=1}")]
    public async Task<IActionResult> ByTag(string? tagName, int currentPage)
    {
        int pageNumber = (await repo.GetPostsCount() / perPage) + 1;
        int skip = currentPage <= 0 ? 0 : (currentPage -1)* perPage;
        var posts = await repo.RetrieveMultiplePosts(tagName ?? string.Empty, skip, perPage);
        var model = new PostsPageViewModel(posts, currentPage, pageNumber, tagName);
        return View("Index", model);
    }


}