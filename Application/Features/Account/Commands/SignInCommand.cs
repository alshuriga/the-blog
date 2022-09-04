using Ardalis.Specification;
using Blog.Application.Features.Account.DTO;
using Blog.Application.Interfaces;
using MediatR;
using System;

namespace Blog.Application.Features.Account.Commands;

public class SignInCommand : IRequest<Unit>
{
    public UserSignInDTO User { get; set; } = null!;
    public class SignInCommandHandler : IRequestHandler<SignInCommand, Unit>
    {
        private readonly IUserService _userService;

        public SignInCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            await _userService.SignInAsync(request.User.Username, request.User.Password);
            return await Unit.Task;
        }
    }
}
