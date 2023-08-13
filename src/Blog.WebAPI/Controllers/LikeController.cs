using Blog.Application.Features.Likes.Requests.Commands;
using Blog.Application.Features.Likes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.WebAPI.Controllers;

[ApiController]
[Route("api/Like/")]
public class LikeController : ControllerBase
{
    private readonly IMediator _mediator;

    public LikeController(IMediator mediator)
    {
        _mediator = mediator;
    }


    ///  <summary>
    ///  like a post (author is a user that is logged in)
    ///  </summary>
    [HttpPost("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize]
    public async Task<IActionResult> Like(long postId)
    {
        var dto = new CreateDeleteLikeDTO() { PostId = postId, Username = User.Identity!.Name! };
        await _mediator.Send(new LikePostCommand(dto));
        return NoContent();
    }

    ///  <summary>
    ///  delete a like of logged in user from a post
    ///  </summary>
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