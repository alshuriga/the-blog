using Blog.Application.Interfaces;
using MediatR;

namespace Blog.Application.Features.User.Requests.Commands;

public class SignOutCommand : IRequest<Unit>
{
    public class SignOutCommandHandler : IRequestHandler<SignOutCommand, Unit>
    {
        private readonly IUserService _userService;

        public SignOutCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public Task<Unit> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            _userService.SignOutAsync();
            return Unit.Task;
        }
    }
}
