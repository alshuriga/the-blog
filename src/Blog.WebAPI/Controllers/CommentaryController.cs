using Blog.Application.Constants;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Posts.Requests.Commands;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebAPI.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CommentaryController : ControllerBase
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
    public async Task<long> Create([FromBody] CreateCommentaryDTO commentary)
    {
        var id = await _mediator.Send(new CreateCommentaryCommand(commentary, commentary.PostId, User.Identity!.Name!));
        return id;
    }

    [HttpPost("{postId:long}")]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Delete([FromBody] long commentaryId, long returnId)
    {
        await _mediator.Send(new DeleteCommentaryCommand(commentaryId));
        return Ok();
    }

}