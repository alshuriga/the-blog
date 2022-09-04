using Blog.Application.Features.Account.DTO;
using Blog.Application.Interfaces;
using MediatR;
using System;

namespace Blog.Application.Features.Account.Commands;

public class SignUpCommand : IRequest<Unit>
{
    public UserSignUpDTO user { get; set; } = null!;
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Unit>
    {
        private readonly IUserService _userService;

        public SignUpCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Unit> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            await _userService.SignUpAsync(request.user.Username, request.user.Email, request.user.Password);
            return await Unit.Task;
        }
    }
}
