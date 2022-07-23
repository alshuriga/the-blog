using Microsoft.AspNetCore.Mvc;

namespace MiniBlog.Controllers;

[AutoValidateAntiforgeryToken]
public class HomeController : Controller
{
    //services
    private IPostsRepo repo;
    private ILogger logger;
    private IHttpContextAccessor context;
    private LinkGenerator linkGenerator;

    //constants
    const int postsPerPage = 5;
    const int commentsPerPage = 5;

    
    public HomeController(IPostsRepo _repo, ILogger<HomeController> _logger, IHttpContextAccessor _context, LinkGenerator _linkGenerator)
    {
        repo = _repo;
        logger = _logger;
        context = _context;
        linkGenerator = _linkGenerator;
    }

    [HttpGet("/{currentPage:int?}")]
    [HttpGet("/tag/{tagName:alpha}/{currentPage:int?}")]
    public async Task<IActionResult> Index(int currentPage = 1, string? tagName = null)
    {
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, postsPerPage, await repo.GetPostsCount());
        PaginateParams paginateParams = new(paginationData?.SkipNumber ?? 0, postsPerPage);
        var posts = tagName is null ? await repo.RetrieveMultiplePosts(paginateParams): await repo.RetrieveMultiplePosts(paginateParams, tagName);

        var model = new MultiplePostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData,
            TagName = tagName
        };
        return View("Index", model);
    }

    [HttpGet("/post/{postId:long}/{currentPage:int?}")]
    public async Task<IActionResult> Post(long postId, int currentPage = 1)
    {
        int commentsCount = await repo.GetCommentsCount(postId);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, commentsPerPage, commentsCount);
        PaginateParams postParams = new(paginationData?.SkipNumber ?? 0, commentsPerPage);
        Post? post = await repo.RetrievePost(postId, postParams);
        if (post == null) return NotFound();
        TempData["postId"] = postId.ToString();
        var model = new SinglePostPageViewModel()
        {
            Post = post,
            CommentsPaginationData = paginationData,
            CommentsCount = commentsCount
        };

        return View(model);

    }

  
    [HttpPost("/post/AddComment")]
    public async Task<IActionResult> AddComment(CommentaryViewModel commentary)
    {   
        long postId;
        if(TempData["postId"] == null || !long.TryParse((string?)TempData["postId"], out postId)) return BadRequest();
        Commentary comment = new Commentary() { Username = commentary.Username, Text = commentary.Text, Email = commentary.Email };
        if (ModelState.IsValid)
        {
            await repo.AddComment(comment, postId);
            return RedirectToAction(nameof(Post), routeValues: new { postId = postId });
        }
        return View();
       
    }

    [HttpPost("/post/delete/{postId:long}")]
    public async Task<IActionResult> DeletePost(long postId)
    {
        if (postId == default(long)) return NotFound();

        await repo.DeletePost(postId);
        return RedirectToAction(nameof(Index));
    }


    [HttpGet("/post/new")]
    [HttpGet("/post/edit/{postId:long}")]
    public async Task<IActionResult> EditPost(long? postId)
    {
        if (postId != null)
        {
            ViewData["title"] = "Edit Post";
            Post? Post = await repo.RetrievePost(postId ?? 0, new PaginateParams());
            if (Post is null) return NotFound();
            string tagString = String.Join(",", Post.Tags.Select(t => t.Name).AsEnumerable());
            Post.Tags.Clear();
            PostEditViewModel model = new() { Post = Post, TagString = tagString };
            return View(model);
        }
        ViewData["title"] = "New Post";
        PostEditViewModel newModel = new() { Post = new Post() };
        return View(newModel);
    }

    [HttpPost("/post/save")]
    public async Task<IActionResult> CreateOrUpdatePost(PostEditViewModel? postModel)
    {
        if (ModelState.IsValid && postModel != null)
        {
            Post post = postModel.Post;
            logger.LogDebug($"Tags passed to controller: " + string.Join(",", post.Tags.Select(t => t.Name).AsEnumerable()));
            if (!String.IsNullOrWhiteSpace(postModel.TagString))
            {
                foreach (string t in postModel.TagString.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    Tag? tag = await repo.CreateOrRetrieveTag(t);
                    if (tag != null)
                    {
                        post.Tags.Add(tag);
                    }
                }
            }
            logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name).AsEnumerable()));
            long? returnId;
            if (post.PostId != 0)
            {
                await repo.UpdatePost(post);
                returnId = post.PostId;
            }
            else
            {
                returnId = await repo.CreatePost(post);
            }
            logger.LogDebug($"Post id (controller-side): {returnId.ToString()}");
            logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name).AsEnumerable()));

            if (returnId is null) return NotFound();
            return RedirectToAction(nameof(Post), new { postId = returnId });

        }
        return View(nameof(Post));
    }

    [HttpPost("/commmentary/delete/{commId:long}")]
    public async Task<IActionResult> DeleteComment(long commId, string? returnId)
    {
        await repo.DeleteComment(commId);
        if (returnId is null) return NotFound();
        return RedirectToAction(nameof(Post), new { postId = returnId });
    }


}