﻿using Blog.Application.Constants;
using Blog.Application.Features.Likes;
using Blog.Application.Features.Likes.Requests.Commands;
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
[Route("api/[controller]")]

public class PostController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreatePostDTO> _validator;

    public PostController(IMediator mediator, IValidator<CreatePostDTO> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }


    ///  <summary>
    ///  list multiple posts (optional filtering by tag or draft) - paginated
    ///  </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [HttpGet("list/{currentPage:int?}")]
    public async Task<IActionResult> List(int currentPage = 0, bool isDraft = false, string? tagName = null)
    {
        if (isDraft && !User.IsInRole(RolesConstants.ADMIN_ROLE)) return Forbid();
        var model = await _mediator.Send(new ListPostsPageQuery(currentPage, isDraft, tagName));
        return Ok(model);
    }

    ///  <summary>
    ///  get a single post with commentaries
    ///  </summary>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{postId:long}")]
    public async Task<PostSingleVM> SinglePost(long postId, int currentPage = 0)
    {
        var includeDrafts = User.IsInRole(RolesConstants.ADMIN_ROLE);
        var model = await _mediator.Send(new GetPostByIdQuery(postId, currentPage, includeDrafts));
        return model;
    }

    ///  <summary>
    ///  update a post
    ///  </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Update(UpdatePostDTO post)
    {
        await _mediator.Send(new UpdatePostCommand(post));
        return NoContent();
    }

    ///  <summary>
    ///  get a post for updating (without commentaries)
    ///  </summary>
    [HttpGet("edit/{postId:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<UpdatePostDTO> Update(long postId)
    {
        var model = await _mediator.Send(new GetPostToEditQuery(postId));
        return model;
    }


    ///  <summary>
    ///  create a post
    ///  </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<long> Create([FromBody] CreatePostDTO post)
    {
        var id = await _mediator.Send(new CreatePostCommand(post));
        return id;
    }

    ///  <summary>
    ///  delete a post by id
    ///  </summary>
    [HttpDelete("{postId:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [Authorize(Roles = RolesConstants.ADMIN_ROLE)]
    public async Task<IActionResult> Delete(long postId)
    {
        await _mediator.Send(new DeletePostCommand(postId));
        return NoContent();
    }


}
