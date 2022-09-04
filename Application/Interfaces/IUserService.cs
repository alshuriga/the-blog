using Blog.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByNameAsync(string userName);
        Task SignInAsync(string username, string password);
        Task SignOutAsync();
        Task SignUpAsync(string username, string email, string password);
    }
}
