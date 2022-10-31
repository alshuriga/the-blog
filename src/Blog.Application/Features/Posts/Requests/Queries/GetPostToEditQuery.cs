using AutoMapper;
using Blog.Application.Exceptions;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;


namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class GetPostToEditQuery : IRequest<UpdatePostDTO>
    {
        private readonly long _id;
        public GetPostToEditQuery(long id)
        {
            _id = id;
        }

        public class GetPostToEditQueryHandler : IRequestHandler<GetPostToEditQuery, UpdatePostDTO>
        {
            private readonly IBlogRepository<Blog.Core.Entities.Post> _repo;
            private readonly IMapper _mapper;
            public GetPostToEditQueryHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<UpdatePostDTO> Handle(GetPostToEditQuery request, CancellationToken cancellationToken)
            {
                var post = await _repo.GetByIdAsync(request._id);
                if (post == null) throw new NotFoundException();
                var result = _mapper.Map<UpdatePostDTO>(post);
                return result;
            }
        }
    }
}
