using Blog.Application.Features.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Posts.ViewModels
{
    public class PostsPageVM
    {
        public IEnumerable<PostListDTO> Posts { get; set; } = new List<PostListDTO>();
        public int CurrentPage { get; set; }
        public int PostsCount { get; set; }

    }
}
