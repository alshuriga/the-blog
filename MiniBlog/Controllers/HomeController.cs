using Microsoft.AspNetCore.Mvc;

namespace MiniBlog.Controllers;

public class HomeController : Controller
{
    const int postsPerPage = 5;
    const int commentsPerPage = 5;
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
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, postsPerPage, await repo.GetPostsCount());
        var posts = await repo.RetrieveMultiplePosts(paginationData?.SkipNumber ?? 0, postsPerPage);
        var model = new MultiplePostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData
        };
        return View("Index", model);
    }

    [HttpGet("/tag/{tagName:alpha}/{currentPage?}")]
    public async Task<ViewResult> ByTag(string tagName, int currentPage = 1)
    {
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, postsPerPage, await repo.GetPostsCount());
        var posts = await repo.RetrieveMultiplePosts(tagName, paginationData?.SkipNumber ?? 0, postsPerPage);
        string? url = linkGenerator.GetPathByAction(context.HttpContext!, "Index");
        var model = new MultiplePostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData,
            TagName = tagName,
            Url = url
        };
        return View("Index", model);
    }

    [HttpGet("/post/{postId:long}/{currentPage:int?}")]
    public async Task<IActionResult> Post(long postId, int currentPage = 1)
    {
        int commentsCount = await repo.GetCommentsCount(postId);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, postsPerPage, commentsCount);
        Post? post = await repo.RetrievePost(postId, paginationData?.SkipNumber ?? 0, commentsPerPage);
        if (post == null) return NotFound();
        string? url = linkGenerator.GetPathByAction(context.HttpContext!, action: nameof(Post), values: new { postId = postId });
        var model = new SinglePostPageViewModel()
        {
            Post = post,
            CommentsPaginationData = paginationData,
            CommentsCount = commentsCount
        };

        return View(model);

    }

    [HttpPost("/post/AddComment/{postId:long}")]
    public async Task<IActionResult> AddComment(CommentaryViewModel model, long postId)
    {
        if (model == null) throw new ArgumentNullException("Commentary model is null");

        Commentary comment = new Commentary() { Username = model.Username, Text = model.Text, Email = model.Email };
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        await repo.AddComment(comment, postId);
        return RedirectToAction(nameof(Post), routeValues: new { postId = postId });

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
        if (postId is not null)
        {
            ViewData["title"] = "Edit Post";
            Post? Post = await repo.RetrievePost(postId.GetValueOrDefault());
            if (Post is null) return NotFound();
            string tagString = String.Join(",", Post.Tags.Select(t => t.Name).AsEnumerable());
            PostEditViewModel model = new() { Post = Post, TagString = tagString };
            return View(model);
        }
        ViewData["title"] = "New Post";
        PostEditViewModel newModel = new() { Post = new Post() };
        return View(newModel);
    }

    [HttpPost("/post/save")]
    public async Task<IActionResult> SavePost(PostEditViewModel? postModel)
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
                        logger.LogDebug($"Post does not contain {tag.Name} tag.. ");
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