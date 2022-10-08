using AutoMapper;
using Blog.Application.Constants;
using Blog.Application.Features.User.DTO;
using Blog.Application.Features.ViewModels;
using Blog.Application.Interfaces;
using MediatR;

namespace Blog.Application.Features.User.Requests.Queries
{
    public class ListUsersQuery : IRequest<UsersListVM>
    {
        public class ListUsersQueryHandler : IRequestHandler<ListUsersQuery, UsersListVM>
        {
            private readonly IUserService _userService;
            private readonly IMapper _mapper;

            public ListUsersQueryHandler(IUserService userService, IMapper mapper)
            {
                _userService = userService;
                _mapper = mapper;
            }

            public async Task<UsersListVM> Handle(ListUsersQuery request, CancellationToken cancellationToken)
            {
                var normals = _mapper.Map<List<UserListDTO>>(await _userService.ListNoRoleUsersAsync());
                var admins = _mapper.Map<List<UserListDTO>>(await _userService.ListUsersAsync(RolesConstants.ADMIN_ROLE));
                return new UsersListVM() { NormalUsers = normals, AdminUsers = admins };
            }
        }

    }
}
