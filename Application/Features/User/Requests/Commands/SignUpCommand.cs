using Blog.Application.Features.User.DTO;
using Blog.Application.Interfaces;
using FluentValidation;
using MediatR;

namespace Blog.Application.Features.User.Requests.Commands;

public class SignUpCommand : IRequest<Unit>
{
    public UserSignUpDTO User { get; set; } = null!;
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, Unit>
    {
        private readonly IUserService _userService;
        private readonly IValidator<UserSignUpDTO> _validator;
        public SignUpCommandHandler(IUserService userService, IValidator<UserSignUpDTO> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        public async Task<Unit> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(request.User);
            await _userService.SignUpAsync(request.User.Username, request.User.Email, request.User.Password);
            return await Unit.Task;
        }
    }
}
