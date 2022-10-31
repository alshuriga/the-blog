using Blog.Application.Interfaces;
using MediatR;

namespace Blog.Application.Features.User.Requests.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        private readonly string _id;
        public DeleteUserCommand(string id)
        {
            _id = id;
        }

        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
        {
            private readonly IUserService _userService;
            public DeleteUserCommandHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                if ((await _userService.ListUsersAsync()).Count() <= 1)
                    throw new ApplicationException("There must be at least one user.");
                await _userService.DeleteUserAsync(request._id);
                return await Unit.Task;
            }
        }
    }
}
