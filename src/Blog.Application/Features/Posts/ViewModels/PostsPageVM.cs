using Blog.Application.Features.Posts.DTO;

namespace Blog.Application.Features.Posts.ViewModels
{
    public class PostsPageVM
    {
        public IEnumerable<PostListVM> Posts { get; set; } = new List<PostListVM>();
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PostsCount { get; set; }

    }
}
