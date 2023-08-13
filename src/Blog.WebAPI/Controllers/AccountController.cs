using Blog.Application.Features.User.Requests.Commands;
using Blog.Application.Features.User.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Blog.Application.Models;

namespace Blog.MVC.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator, IOptions<JwtOptions> jwtOptions)
        {
            _mediator = mediator;
        }


        ///  <summary>
        ///  sign in to an account
        ///  </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login([FromBody] UserSignInDTO user)
        {
           var token = await _mediator.Send(new SignInJwtCommand(user));
           return Ok(token);
        }


        ///  <summary>
        ///  sign out of an account
        ///  </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout()
        {
            await _mediator.Send(new SignOutCommand());
            return NoContent();
        }


        ///  <summary>
        ///  create new account
        ///  </summary>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> SignUp([FromBody]UserSignUpDTO user)
        {
            await _mediator.Send(new SignUpCommand(user));
            return NoContent();
        }

    }
}
