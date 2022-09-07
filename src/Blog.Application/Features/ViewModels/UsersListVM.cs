using Blog.Application.Features.User.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.ViewModels
{
    public class UsersListVM
    {
        public List<UserListDTO> NormalUsers { get; set; } = new();
        public List<UserListDTO> AdminUsers { get; set; } = new();
    }
}
