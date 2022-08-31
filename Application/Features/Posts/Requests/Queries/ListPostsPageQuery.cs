using AutoMapper;
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
                
                var posts = await _repo.ListAsync(new PostsByPageSpecification(request.CurrentPage, request.TagName));
                var dtos = _mapper.Map<IEnumerable<PostListDTO>>(posts);
                PostsPageVM viewModel = new PostsPageVM
                {
                    CurrentPage = request.CurrentPage,
                    PostsCount = await _repo.CountAsync(),
                    Posts = dtos
                };
                return viewModel;
            }
        }
    }
}
