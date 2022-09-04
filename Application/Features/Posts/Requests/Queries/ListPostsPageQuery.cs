using AutoMapper;
using Blog.Application.Constants;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.Specifications;
using Blog.Application.Features.Posts.ViewModels;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class ListPostsPageQuery : IRequest<PostsPageVM>
    {
        public int CurrentPage { get; set; }
        public string? TagName { get; set; }
        public bool IsDraft { get; set; } 

        public class ListPostsPageQueryHandler : IRequestHandler<ListPostsPageQuery, PostsPageVM>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;
            public ListPostsPageQueryHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<PostsPageVM> Handle(ListPostsPageQuery request, CancellationToken cancellationToken)
            {
                var listedPosts = (await _repo
                    .ListAsync(new PostsSpecification(currentPage: request.CurrentPage, tagName: request.TagName, isDraft: request.IsDraft)))
                    .OrderByDescending(p => p.DateTime).ToList();

                var dtos = _mapper.Map<IEnumerable<PostListVM>>(listedPosts);
                var postsCount = await _repo.CountAsync(new PostsSpecification(isDraft: request.IsDraft, tagName: request.TagName));
                PostsPageVM viewModel = new PostsPageVM
                {
                    CurrentPage = request.CurrentPage,
                    PageCount = (int)Math.Ceiling(postsCount/ ((double)PaginationConstants.POSTS_PER_PAGE)),
                    PostsCount = postsCount,
                    Posts = dtos
                };
                return viewModel;
            }
        }
    }
}
