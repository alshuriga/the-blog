using Blog.Application.Constants;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Posts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Posts.ViewModels
{
    public class PostSingleVM
    {
        public PostDTO Post { get; set; } = null!;
        public IReadOnlyList<CommentaryDTO> Commentaries { get; set; } = null!; 
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public CreateCommentaryDTO CommentaryDTO { get; set; } = new CreateCommentaryDTO();
    }
}
