using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using MediatR;
using Blog.Core.Entities;
using AutoMapper;

namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class GetSinglePostByIdQuery : IRequest<PostDTO?>
    {
        public long Id { get; set; }

        public class GetSinglePostQueryHandler : IRequestHandler<GetSinglePostByIdQuery, PostDTO?>
        {
            private readonly IBlogRepository<Blog.Core.Entities.Post> _repo;
            private readonly IMapper _mapper;
            public GetSinglePostQueryHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<PostDTO?> Handle(GetSinglePostByIdQuery request, CancellationToken cancellationToken)
            {
                Post? post =  await _repo.GetByIdAsync(request.Id);
                PostDTO? postDto = _mapper.Map<PostDTO?>(post);
                return postDto;
            }
        }
    }
}
