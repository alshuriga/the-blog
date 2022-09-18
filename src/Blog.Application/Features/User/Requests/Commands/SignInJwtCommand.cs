
using Blog.Application.Constants;
using Blog.Application.Features.User.DTO;
using Blog.Application.Interfaces;
using Blog.Application.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Blog.Application.Features.User.Requests.Commands;

public class SignInJwtCommand : IRequest<string>
{
    private readonly UserSignInDTO _user;
    

    public SignInJwtCommand(UserSignInDTO user)
    {
        _user = user;
    }

    public class SignInJwtCommandHandler : IRequestHandler<SignInJwtCommand, string>
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserSignInDTO> _validator;
        private readonly JwtOptions _jwtOptions;


        public SignInJwtCommandHandler(IUserService userService, IValidator<UserSignInDTO> validator, IOptions<JwtOptions> jwtOptions)
        {
            _userService = userService;
            _validator = validator;
            _jwtOptions = jwtOptions.Value;
        }


        public async Task<string> Handle(SignInJwtCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request._user);
            var result = await _userService.CheckPasswordAsync(request._user.Username, request._user.Password);
            if (!result) throw new ValidationException(new ValidationFailure[] { new("", "Username or/and password is incorrect") });
            var userRoles = string.Join(",", 
                (await _userService.GetUserByNameAsync(request._user.Username))?.Roles ?? Enumerable.Empty<string>());
            
            var issuer = _jwtOptions.Issuer;
            var audience = _jwtOptions.Audience;
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);

            var decriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, request._user.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, userRoles)
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
            };

            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = handler.CreateToken(decriptor);
            return (handler.WriteToken(token));
        }
    }
}
