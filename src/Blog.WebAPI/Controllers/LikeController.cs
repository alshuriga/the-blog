using Blog.Application.Features.Likes.Requests.Commands;
using Blog.Application.Features.Likes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebAPI.Controllers;

[ApiController]
[Route("api/Post/[action]")]
public class LikeController : ControllerBase
{
    private readonly IMediator _mediator;

    public LikeController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> Like(long postId)
    {
        var dto = new CreateDeleteLikeDTO() { PostId = postId, Username = User.Identity!.Name! };
        await _mediator.Send(new LikePostCommand(dto));
        return NoContent();
    }

    [HttpDelete("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> UnLike(long postId)
    {
        var dto = new CreateDeleteLikeDTO() { PostId = postId, Username = User.Identity!.Name! };
        await _mediator.Send(new UnlikePostCommand(dto));
        return NoContent();
    }

}