using Blog.Application.Features.Account.Commands;
using Blog.Application.Features.Account.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login([FromForm] UserSignInDTO user, string returnUrl)
        {
           await _mediator.Send(new SignInCommand() { User = user});
           return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await _mediator.Send(new SignOutCommand());
            return Redirect(returnUrl);
        }

    }
}
