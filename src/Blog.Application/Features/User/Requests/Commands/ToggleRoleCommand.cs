using Blog.Application.Interfaces;
using MediatR;

namespace Blog.Application.Features.User.Requests.Commands
{
    public class ToggleRoleCommand : IRequest<Unit>
    {
        private readonly string _id;
        private readonly string _rolename;
        public ToggleRoleCommand(string id, string rolename)
        {
            _id = id;
            _rolename = rolename;
        }

        public class ToggleRoleCommandHandler : IRequestHandler<ToggleRoleCommand, Unit>
        {
            private readonly IUserService _userService;

            public ToggleRoleCommandHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<Unit> Handle(ToggleRoleCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByIdAsync(request._id);
                if (user.Roles.Contains(request._rolename))
                {
                    if ((await _userService.ListUsersAsync(request._rolename)).Count() <= 1)
                    {
                        throw new ApplicationException("There must be at least one user with admin rights.");
                    }
                    await _userService.RemoveFromRoleAsync(request._id, request._rolename);
                }
                else
                {
                    await _userService.AddToRoleAsync(request._id, request._rolename);
                }
                return await Unit.Task;
            }
        }

    }
}
