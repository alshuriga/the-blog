using Blog.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.User.Requests.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public string Id { get; set; } = null!;
        
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
                await _userService.DeleteUserAsync(request.Id);
                return await Unit.Task;
            }
        }
    }
}
