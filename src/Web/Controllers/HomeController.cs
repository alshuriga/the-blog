using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MiniBlog.Core.Models;
using MiniBlog.Web.Filters;
using MiniBlog.Web.ViewModels;
using Ardalis.Specification;

namespace MiniBlog.Web.Controllers;

[AutoValidateAntiforgeryToken]
[ServiceFilter(typeof(ApplicationExceptionFilter))]
public class HomeController : Controller
{
    //services
    private readonly IMiniBlogRepo _repo;
    private readonly ILogger _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUnitOfWork _unit;

    //constants
    const int PostsPerPage = 5;
    const int CommentsPerPage = 5;


    public HomeController(IMiniBlogRepo repo, ILogger<HomeController> logger, UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
    {
        this._userManager = userManager;
        this._repo = repo;
        this._logger = logger;
        this._unit = unitOfWork;
    }

    [HttpGet("/{currentPage:int?}")]
    [HttpGet("/tag/{tagName:alpha}/{currentPage:int?}")]
    public async Task<IActionResult> Index(int currentPage = 1, string? tagName = null)
    {
        Tag? tag = tagName == null ? null : new() { Name = tagName };
        ISpecification<Post> postsSpecification = new PostsByPageSpecification(currentPage, tag, true);

        int postsCount = tag == null
        ? await _unit.postReadRepo.CountAsync()
        : await _unit.postReadRepo.CountAsync(new PostsByTagSpecification(tag));

        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, MiniBlog.Constants.PaginationConstants.POSTS_PER_PAGE, postsCount);
        var posts = ((await _unit.postReadRepo.ListAsync(postsSpecification)).Select(p => new PostPartialViewModel
        {
            Post = new PostDto
            {
                Id = p.Id,
                Header = p.Header,
                DateTime = p.DateTime,
                Text = p.Text
            },
            TagNames = p.Tags.Select(t => t.Name),
            CommentsButton = false,
            CommentariesCount = _repo.GetCommentariesCount(p.Id).Result
        }));

        var model = new PostsIndexViewModel
        {
            Posts = posts,
            PaginationData = paginationData,
            TagName = tagName,
            PostsCount = postsCount,
        };

        return View("Index", model);
    }

    [HttpGet("/post/{postId:long}/{currentPage:int?}")]
    public async Task<IActionResult> ShowPost(long postId, int currentPage = 1)
    {
        int commentsCount = await _repo.GetCommentariesCount(postId);
        PaginationData? paginationData = PaginationData.CreatePaginationDataOrNull(currentPage, CommentsPerPage, commentsCount);
        PaginateParams postParams = new(paginationData?.SkipNumber ?? 0, CommentsPerPage);
        Post? post = await _repo.RetrievePost(postId, postParams);
        if (post == null) return NotFound();
        _logger.LogDebug($"Comments: {post.Commentaries.Any()}");
        TempData["postId"] = postId.ToString();
        var model = new PostViewModel()
        {
            PostPartial = new()
            {
                Post = new()
                {
                    Id = post.Id,
                    Header = post.Header,
                    Text = post.Text,
                    DateTime = post.DateTime
                },
                CommentsButton = false,
                TagNames = post.Tags.Select(t => t.Name)
            },
            Commentaries = post.Commentaries.Select(c => new CommentaryDto
            {
                Id = c.Id,
                Username = c.Username,
                Email = c.Email,
                Text = c.Text,
                DateTime = c.DateTime
            }),
            CommentsPaginationData = paginationData
        };

        _logger.LogDebug($"Comments in model: {string.Join(", ", model.Commentaries.Select(c => c.Id))}");
        return View(model);
    }

    [Authorize]
    [HttpPost("/AddComment/{postid:long}")]
    public async Task<IActionResult> AddComment(CommentaryDto commentary, long postId)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user != null)
            {
                Commentary comment = new Commentary() { Username = user.UserName, Email = user.Email, Text = commentary.Text };
                await _repo.CreateComment(comment, postId);
            }
        }
        return RedirectToAction(nameof(ShowPost), routeValues: new { postId = postId });
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/post/delete/{postId:long}")]
    public async Task<IActionResult> DeletePost(long postId)
    {
        await _repo.DeletePost(postId);
        return RedirectToAction(nameof(Index));
    }


    [Authorize(Roles = "Admins")]
    [HttpGet("/post/new")]
    [HttpGet("/post/edit/{postId:long}")]
    public async Task<IActionResult> EditPost(long postId = default(long))
    {
        if (postId != default(long))
        {
            ViewData["title"] = "Edit Post";
            Post? post = await _repo.RetrievePost(postId, new PaginateParams());
            if (post is null) return NotFound();
            string tagString = String.Join(",", post.Tags.Select(t => t.Name).AsEnumerable());
            PostEditViewModel model = new()
            {
                Post = new PostDto
                {
                    Id = post.Id,
                    Header = post.Header,
                    Text = post.Text,
                    DateTime = post.DateTime
                },
                TagString = tagString
            };
            return View(model);
        }
        ViewData["title"] = "New Post";
        PostEditViewModel newModel = new() { Post = new PostDto() };
        return View(newModel);
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/post/save")]
    public async Task<IActionResult> SavePost(PostEditViewModel? postModel)
    {
        string[] tagNames = postModel?.TagString?.Split(",", StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        if (tagNames.Length > 5) ModelState.AddModelError(nameof(postModel.TagString), "Maximum number of tags is 5");
        if (ModelState.IsValid && postModel != null)
        {
            Post post = new()
            {
                Id = postModel.Post.Id,
                Header = postModel.Post.Header,
                Text = postModel.Post.Text,
                DateTime = postModel.Post.DateTime,
                Tags = new List<Tag>()
            };

            _logger.LogDebug($"Tags passed to controller: " + string.Join(",", post.Tags.Select(t => t.Name)));
            foreach (string t in tagNames)
            {
                string tagName = t.Trim();
                Tag tag = await _repo.RetrieveTagByName(tagName) ?? new() { Name = tagName };
                post.Tags.Add(tag);
            }
            _logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name)));
            long? returnId;
            if (post.Id != default(long))
            {
                await _repo.UpdatePost(post);
                returnId = post.Id;
            }
            else
            {
                returnId = await _repo.CreatePost(post);
            }
            _logger.LogDebug($"Post id (controller-side): {returnId.ToString()}");
            _logger.LogDebug($"Tags after modifying: " + string.Join(",", post.Tags.Select(t => t.Name)));

            if (returnId is null) throw new MiniBlogWebException("Error while saving a post");
            return RedirectToAction(nameof(ShowPost), new { postId = returnId });

        }
        else
        {
            return View(nameof(EditPost), postModel);
        }
    }

    [Authorize(Roles = "Admins")]
    [HttpPost("/commmentary/delete/{commId:long}")]
    public async Task<IActionResult> DeleteComment(long commId, long returnId)
    {
        await _repo.DeleteComment(commId);
        return RedirectToAction(nameof(ShowPost), new { postId = returnId });
    }

}