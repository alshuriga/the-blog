using Ardalis.Specification;
using Blog.Application.Features.User.DTO;
using Blog.Application.Interfaces;
using MediatR;
using System;

namespace Blog.Application.Features.User.Requests.Commands;

public class SignInCommand : IRequest<Unit>
{
    private readonly UserSignInDTO _user;

    public SignInCommand(UserSignInDTO user)
    {
        _user = user;
    }
  
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Unit>
    {
        private readonly IUserService _userService;

        public SignInCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            await _userService.SignInAsync(request._user.Username, request._user.Password);
            return await Unit.Task;
        }
    }
}
