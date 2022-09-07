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
        private readonly int _currentPage;
        private readonly string? _tagName;
        private readonly bool _isDraft;

        public ListPostsPageQuery(int currentPage, bool isDraft, string? tagName = null)
        {
            _currentPage = currentPage;
            _isDraft = isDraft;
            _tagName = tagName;
        }

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
                    .ListAsync(new PostsSpecification(currentPage: request._currentPage, tagName: request._tagName, isDraft: request._isDraft)))
                    .OrderByDescending(p => p.DateTime).ToList();

                var dtos = _mapper.Map<IEnumerable<PostListVM>>(listedPosts);
                var postsCount = await _repo.CountAsync(new PostsSpecification(isDraft: request._isDraft, tagName: request._tagName));
                PostsPageVM viewModel = new PostsPageVM
                {
                    CurrentPage = request._currentPage,
                    PageCount = (int)Math.Ceiling(postsCount/ ((double)PaginationConstants.POSTS_PER_PAGE)),
                    PostsCount = postsCount,
                    Posts = dtos
                };
                return viewModel;
            }
        }
    }
}
