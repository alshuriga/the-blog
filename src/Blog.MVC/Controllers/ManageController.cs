using Blog.Application.Constants;
using Blog.Application.Features.User.Requests.Commands;
using Blog.Application.Features.User.Requests.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers;

[Authorize(Roles = RolesConstants.ADMIN_ROLE)]
public class ManageController : Controller
{
    private readonly IMediator _mediator;
    public ManageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> ToggleAdminRole(string userId)
    {
        await _mediator.Send(new ToggleRoleCommand(userId, RolesConstants.ADMIN_ROLE));
        return RedirectToAction(nameof(UserList));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        await _mediator.Send(new DeleteUserCommand(userId));
        return RedirectToAction(nameof(UserList));
    }

    [HttpGet]
    public async Task<IActionResult> UserList()
    {
        var model = await _mediator.Send(new ListUsersQuery());
        return View(model);
    }
}
