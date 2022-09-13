
using Blog.Application.Features.User.DTO;
using Blog.Application.Interfaces;
using FluentValidation;
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
        private readonly IValidator<UserSignInDTO> _validator;

        public SignInCommandHandler(IUserService userService, IValidator<UserSignInDTO> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request._user);
            await _userService.SignInAsync(request._user.Username, request._user.Password);
            return await Unit.Task;
        }
    }
}
