using Microsoft.AspNetCore.Mvc;


namespace MiniBlog.Controllers;

public class HomeController : Controller
{

    const int perPage = 5;
    private IPostsRepo repo;

    private ILogger logger;

    private IHttpContextAccessor context;

    private LinkGenerator linkGenerator;

    public HomeController(IPostsRepo _repo, ILogger<HomeController> _logger, IHttpContextAccessor _context, LinkGenerator _linkGenerator)
    {
        repo = _repo;
        logger = _logger;
        context = _context;
        linkGenerator = _linkGenerator;
    }

    [HttpGet("/{currentPage?}")]
    public async Task<ViewResult> Index(int currentPage = 1)
    {
        int skip = currentPage <= 0 ? 0 : (currentPage - 1) * perPage;
        int count = await repo.GetPostsCount();
        var posts = await repo.RetrieveMultiplePosts(skip, perPage);
        int pageNumber = (int)Math.Ceiling(count / (float)perPage);
        var paginationData = new PaginationData() { CurrentPage = currentPage, PageNumber = pageNumber };
        var model = new PostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData
        };
        return View("Index", model);
    }

    [HttpGet("/tag/{tagName:alpha}/{currentPage?}")]
    public async Task<ViewResult> ByTag(string tagName, int currentPage = 1)
    {
        int skip = currentPage <= 0 ? 0 : (currentPage - 1) * perPage;
        int count = await repo.GetPostsCount(tagName);
        var posts = await repo.RetrieveMultiplePosts(tagName, skip, perPage);
        int pageNumber = (int)Math.Ceiling(count / (float)perPage);
        string? url = linkGenerator.GetPathByAction(context.HttpContext!, "Index");
        var paginationData = new PaginationData() { CurrentPage = currentPage, PageNumber = pageNumber };
        var model = new PostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData,
            TagName = tagName,
            Url = url
        };
        return View("Index", model);
    }

    [HttpGet("/post/{postId:long}")]
    public async Task<IActionResult> Post(long postId)
    {
        Post? post = await repo.RetrievePost(postId);
        if (post == null) return NotFound();
        return View(post);

    }
    
    [HttpPost("/post/{postId:long}")]
    public async Task<IActionResult> Post(Commentary? comment, long postId)
    {
        logger.LogDebug($"POST method. ModelState: {ModelState.IsValid}");
        logger.LogDebug(String.Join(",", ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage).AsEnumerable())));
        logger.LogDebug($"Comment Username: {comment?.Username}\nComment Text: {comment?.Text}\nComment Email: {comment?.Email}\nComment DateTime: {comment?.DateTime}\n");
        if (comment != null && ModelState.IsValid)
        {
            await repo.AddComment(comment, postId);
            RedirectToAction("Post", postId);
        }
        return View(nameof(Post), await repo.RetrievePost(postId));
    }

}