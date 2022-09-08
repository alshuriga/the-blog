using Blog.Application.Constants;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Requests.Commands;
using Blog.Application.Features.Posts.Requests.Queries;
using Blog.MVC.Filters;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers;

public class PostController : Controller
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreatePostDTO> _validator;

    public PostController(IMediator mediator, IValidator<CreatePostDTO> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("{currentPage:int?}")]
    public async Task<IActionResult> List(int currentPage = 0, bool isDraft = false, string? tagName = null)
    {
        if (isDraft && !User.IsInRole(RolesConstants.ADMIN_ROLE)) throw new ApplicationException("Access Denied");
        ViewData["header"] = tagName == null ? "" : $"with {tagName} tag";
        var model = await _mediator.Send(new ListPostsPageQuery(currentPage, isDraft, tagName));
        return View(model);
    }

    public async Task<IActionResult> SinglePost(long postId, int currentPage = 0)
    {
        var model = await _mediator.Send(new GetPostByIdQuery(postId, currentPage));
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Update(long postId)
    {
        var model = await _mediator.Send(new GetPostToEditQuery(postId));
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Create(CreatePostDTO post)
    {
        var id = await _mediator.Send(new CreatePostCommand(post));
        return RedirectToAction("SinglePost", new { postId = id });
    }

    [HttpPost]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Update(UpdatePostDTO post)
    {
        await _mediator.Send(new UpdatePostCommand(post));
        return RedirectToAction("SinglePost", new { postId = post.PostId });
    }
}
