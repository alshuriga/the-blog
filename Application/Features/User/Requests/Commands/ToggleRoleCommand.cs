using Blog.Application.Interfaces;
using MediatR;

namespace Blog.Application.Features.User.Requests.Commands
{
    public class ToggleRoleCommand : IRequest<Unit>
    {
        public string Id { get; set; } = null!;
        public string Rolename { get; set; } = null!;

        public class ToggleRoleCommandHandler : IRequestHandler<ToggleRoleCommand, Unit>
        {
            private readonly IUserService _userService;

            public ToggleRoleCommandHandler(IUserService userService)
            {
                _userService = userService;
            }

            public async Task<Unit> Handle(ToggleRoleCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetUserByIdAsync(request.Id);
                if (user.Roles.Contains(request.Rolename))
                {
                    if ((await _userService.ListUsersAsync(request.Rolename)).Count() <= 1)
                    {
                        throw new ApplicationException("There must be at least one user with admin rights.");
                    }
                    await _userService.RemoveFromRoleAsync(request.Id, request.Rolename);
                }
                else
                {
                    await _userService.AddToRoleAsync(request.Id, request.Rolename);
                }
                return await Unit.Task;
            }
        }

    }
}
