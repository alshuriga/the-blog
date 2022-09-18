using Blog.Application.Features.User.Requests.Commands;
using Blog.Application.Features.User.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Blog.Application.Constants;
using Blog.Application.Models;

namespace Blog.MVC.Controllers
{
    
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator, IOptions<JwtOptions> jwtOptions)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserSignInDTO user)
        {
           var token = await _mediator.Send(new SignInJwtCommand(user));
           return Ok(token);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new SignOutCommand());
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody]UserSignUpDTO user)
        {
            await _mediator.Send(new SignUpCommand(user));
            return Ok();
        }

    }
}
