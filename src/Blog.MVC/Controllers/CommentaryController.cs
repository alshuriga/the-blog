using Blog.Application.Constants;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Posts.Requests.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers;


public class CommentaryController : Controller
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateCommentaryDTO> _validator;

    public CommentaryController(IMediator mediator, IValidator<CreateCommentaryDTO> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }


    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromForm] CreateCommentaryDTO commentary)
    {
        await _mediator.Send(new CreateCommentaryCommand(commentary, commentary.PostId, User.Identity!.Name!));
        return StatusCode(StatusCodes.Status308PermanentRedirect, Url.Action("SinglePost", "Post", new { postId = commentary.PostId }) ?? "/");
    }

    [Authorize]
    public IActionResult Create(long postId)
    {
        return View(new CreateCommentaryDTO() { PostId = postId });
    }



    [HttpPost]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Delete([FromForm] long commentaryId, long returnId)
    {
        await _mediator.Send(new DeleteCommentaryCommand(commentaryId));
        return RedirectToAction("SinglePost", "Post", new { postId = returnId });
    }

}