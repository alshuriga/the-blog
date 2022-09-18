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
[Route("[controller]/[action]")]
public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreatePostDTO> _validator;

    public PostController(IMediator mediator, IValidator<CreatePostDTO> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpGet("{currentPage:int?}")]
    public async Task<PostsPageVM> List(int currentPage = 0, bool isDraft = false, string? tagName = null)
    {
        if (isDraft && !User.IsInRole(RolesConstants.ADMIN_ROLE)) throw new ApplicationException("Access Denied");
        var model = await _mediator.Send(new ListPostsPageQuery(currentPage, isDraft, tagName));
        return model;
    }

    [HttpGet("{postId:long}")]
    public async Task<PostSingleVM> SinglePost(long postId, int currentPage = 0)
    {
        var includeDrafts = User.IsInRole(RolesConstants.ADMIN_ROLE);
        var model = await _mediator.Send(new GetPostByIdQuery(postId, currentPage, includeDrafts));
        return model;
    }

    [HttpPost]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Update(UpdatePostDTO post)
    {
        await _mediator.Send(new UpdatePostCommand(post));
        return Ok();
    }

    [HttpGet("{postId:long}")]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<UpdatePostDTO> Update(long postId)
    {
        var model = await _mediator.Send(new GetPostToEditQuery(postId));
        return model;
    }

    [HttpPost]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<long> Create([FromBody] CreatePostDTO post)
    {
        var id = await _mediator.Send(new CreatePostCommand(post));
        return id;
    }


    [HttpPost("{postId:long}")]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Delete(long postId)
    {
        await _mediator.Send(new DeletePostCommand(postId));
        return Ok();
    }
}
