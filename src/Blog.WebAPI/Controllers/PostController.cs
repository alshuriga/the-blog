using Blog.Application.Constants;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Requests.Commands;
using Blog.Application.Features.Posts.Requests.Queries;
using Blog.Application.Features.Posts.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]

public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreatePostDTO> _validator;

    public PostController(IMediator mediator, IValidator<CreatePostDTO> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("{currentPage:int?}")]
    public async Task<IActionResult> List(int currentPage = 0, bool isDraft = false, string? tagName = null)
    {
        if (isDraft && !User.IsInRole(RolesConstants.ADMIN_ROLE)) return Unauthorized();
        var model = await _mediator.Send(new ListPostsPageQuery(currentPage, isDraft, tagName));
        return Ok(model);
    }


    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{postId:long}")]
    public async Task<PostSingleVM> SinglePost(long postId, int currentPage = 0)
    {
        var includeDrafts = User.IsInRole(RolesConstants.ADMIN_ROLE);
        var model = await _mediator.Send(new GetPostByIdQuery(postId, currentPage, includeDrafts));
        return model;
    }


    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Update(UpdatePostDTO post)
    {
        await _mediator.Send(new UpdatePostCommand(post));
        return NoContent();
    }


    [HttpGet("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<UpdatePostDTO> Update(long postId)
    {
        var model = await _mediator.Send(new GetPostToEditQuery(postId));
        return model;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<long> Create([FromBody] CreatePostDTO post)
    {
        var id = await _mediator.Send(new CreatePostCommand(post));
        return id;
    }


    [HttpDelete("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Delete(long postId)
    {
        await _mediator.Send(new DeletePostCommand(postId));
        return NoContent();
    }
}
