using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Likes;
using Blog.Application.Features.Posts.DTO;

namespace Blog.Application.Features.Posts.ViewModels
{
    public class PostSingleVM
    {
        public PostDTO Post { get; set; } = null!;
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public CreateCommentaryDTO CommentaryDTO { get; set; } = new CreateCommentaryDTO();
        public IReadOnlyList<CommentaryDTO> Commentaries { get; set; } = null!;
        public List<LikeDTO> Likes = new List<LikeDTO>();
    }
}
