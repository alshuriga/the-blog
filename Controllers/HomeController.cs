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

    [HttpGet("/{currentPage?}")]
    [HttpGet("/tag/{tagName:alpha}/{currentPage?}")]
    public async Task<ViewResult> Index(string? tagName, int currentPage = 1)
    {   
        int skip = currentPage <= 0 ? 0 : (currentPage - 1) * perPage;
        int count = (tagName is null) ? await repo.GetPostsCount() : await repo.GetPostsCount(tagName);
        var posts = (tagName is null) ? await repo.RetrieveMultiplePosts(skip, perPage): await repo.RetrieveMultiplePosts(tagName, skip, perPage);
        int pageNumber = (int)Math.Ceiling(count / (float)perPage);
        logger.LogDebug($"Page number: {pageNumber}\nCurrent page: {currentPage}, TagName: {tagName}");
        var paginationData = new PaginationData() { CurrentPage = currentPage, PageNumber = pageNumber };
        var model = new PostsPageViewModel(posts, paginationData, tagName);
        return View("Index", model);
    }

}