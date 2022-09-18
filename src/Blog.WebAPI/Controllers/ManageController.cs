using Blog.Application.Constants;
using Blog.Application.Features.User.Requests.Commands;
using Blog.Application.Features.User.Requests.Queries;
using Blog.Application.Features.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize(Roles = RolesConstants.ADMIN_ROLE)]
public class ManageController : Controller
{
    private readonly IMediator _mediator;
    public ManageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> ToggleAdminRole(string userId)
    {
        await _mediator.Send(new ToggleRoleCommand(userId, RolesConstants.ADMIN_ROLE));
        return Ok();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));
        return Ok();
    }

    [HttpGet]
    public async Task<UsersListVM> UserList()
    {
        var model = await _mediator.Send(new ListUsersQuery());
        return model;
    }
}
