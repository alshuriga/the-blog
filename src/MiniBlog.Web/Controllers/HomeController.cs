using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Core.Models;
using MiniBlog.Web.Filters;
using MiniBlog.Web.ViewModels;

namespace MiniBlog.Web.Controllers;

[AutoValidateAntiforgeryToken]
[ServiceFilter(typeof(ApplicationExceptionFilter))]
public class HomeController : Controller
{
    //services
    private readonly IPostsRepo _repo;
    private readonly ILogger _logger;
    private readonly UserManager<IdentityUser> _userManager;

    //constants
    const int PostsPerPage = 5;
    const int CommentsPerPage = 5;


    public HomeController(IPostsRepo repo, ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
    {
        this._userManager = userManager;
        this._repo = repo;
        this._logger = logger;
    }

    [HttpGet("/{currentPage:int?}")]
    [HttpGet("/tag/{tagName:alpha}/{currentPage:int?}")]
    public async Task<IActionResult> Index(int currentPage = 1, string? tagName = null)
    {
        int postsCount = tagName == null ? await _repo.GetPostsCount() : await _repo.GetPostsCount(tagName);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, PostsPerPage, postsCount);
        PaginateParams paginateParams = new(paginationData?.SkipNumber ?? 0, PostsPerPage);
        var posts = tagName is null ? await _repo.RetrievePostsRange(paginateParams) : await _repo.RetrievePostsRange(paginateParams, tagName);

        var model = new MultiplePostsPageViewModel
        {
            Posts = posts,
            PaginationData = paginationData,
            TagName = tagName,
            PostsCount = postsCount
        };
        return View("Index", model);
    }

    [HttpGet("/post/{postId:long}/{currentPage:int?}")]
    public async Task<IActionResult> Post(long postId, int currentPage = 1)
    {
        int commentsCount = await _repo.GetCommentariesCount(postId);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, CommentsPerPage, commentsCount);
        PaginateParams postParams = new(paginationData?.SkipNumber ?? 0, CommentsPerPage);
        Post? post = await _repo.RetrievePost(postId, postParams);
        if (post == null) return NotFound();
        _logger.LogDebug($"Comments: {post.Commentaries.Any()}");
        TempData["postId"] = postId.ToString();
        var model = new SinglePostPageViewModel()
        {
            Post = post,
            CommentsPaginationData = paginationData,
            CommentsCount = commentsCount
        };
        _logger.LogDebug($"Comments in model: {string.Join(", ", model.Post.Commentaries.Select(c => c.CommentaryId))}");
        return View(model);
    }

    [Authorize]
    [HttpPost("/AddComment/{postid:long}")]
    public async Task<IActionResult> AddComment(CommentaryViewModel commentary, long postId)
    {
        if (ModelState.IsValid)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                Commentary comment = new Commentary() { Username = user.UserName, Email = user.Email, Text = commentary.Text };
                await _repo.CreateComment(comment, postId);
            }
        }
        return RedirectToAction(nameof(Post), routeValues: new { postId = postId });
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/post/delete/{postId:long}")]
    public async Task<IActionResult> DeletePost(long postId)
    {
        if (postId == default(long)) return NotFound();

        await _repo.DeletePost(postId);
        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Admins")]
    [HttpGet("/post/new")]
    [HttpGet("/post/edit/{postId:long}")]
    public async Task<IActionResult> EditPost(long? postId)
    {
        if (postId != null)
        {
            ViewData["title"] = "Edit Post";
            Post? post = await _repo.RetrievePost(postId ?? 0, new PaginateParams());
            if (post is null) return NotFound();
            string tagString = String.Join(",", post.Tags.Select(t => t.Name).AsEnumerable());
            PostEditViewModel model = new() { Post = post, TagString = tagString };
            return View(model);
        }
        ViewData["title"] = "New Post";
        PostEditViewModel newModel = new() { Post = new Post() };
        return View(newModel);
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/post/save")]
    public async Task<IActionResult> CreateOrUpdatePost(PostEditViewModel? postModel)
    {
        string[]? tagNames = postModel?.TagString?.Split(",", StringSplitOptions.RemoveEmptyEntries);
        if (tagNames == null || tagNames.Length > 5) ModelState.AddModelError(nameof(postModel.TagString), "Maximum number of tags is 5");
        if (ModelState.IsValid && postModel != null)
        {
            Post post = postModel.Post;
            post.Tags.Clear();
            _logger.LogDebug($"Tags passed to controller: " + string.Join(",", post.Tags.Select(t => t.Name)));
            if (!String.IsNullOrWhiteSpace(postModel.TagString))
            {
                foreach (string t in postModel.TagString.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    string tagName = t.Trim();
                    await _repo.CreateTagIfNotExist(new Tag { Name = tagName });
                    Tag? tag = await _repo.RetrieveTagByName(tagName);
                    if(tag != null) post.Tags.Add(tag);
                }
            }
            _logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name)));
            long? returnId;
            if (post.PostId != 0)
            {
                await _repo.UpdatePost(post);
                returnId = post.PostId;
            }
            else
            {
                returnId = await _repo.CreatePost(post);
            }
            _logger.LogDebug($"Post id (controller-side): {returnId.ToString()}");
            _logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name)));

            if (returnId is null) return NotFound();
            return RedirectToAction(nameof(Post), new { postId = returnId });

        }
        else
        {
            return View(nameof(EditPost), postModel);
        }
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/commmentary/delete/{commId:long}")]
    public async Task<IActionResult> DeleteComment(long commId, long? returnId)
    {
        await _repo.DeleteComment(commId);
        if (returnId is default(long)) return RedirectToAction(nameof(Index));
        return RedirectToAction(nameof(Post), new { postId = returnId });
    }

}