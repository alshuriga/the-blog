using AutoMapper;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Likes.Requests.Commands
{
    public class LikePostCommand : IRequest<Unit>
    {
        private readonly CreateDeleteLikeDTO _createLikeDTO;
        

        public LikePostCommand(CreateDeleteLikeDTO createLikeDTO)
        {
            _createLikeDTO = createLikeDTO;
        }

        public class LikePostCommandHandler : IRequestHandler<LikePostCommand, Unit>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;

            public LikePostCommandHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }   

            public async Task<Unit> Handle(LikePostCommand request, CancellationToken cancellationToken)
            {
                var post = await _repo.GetByIdAsync(request._createLikeDTO.PostId);
                if (post == null) return await Unit.Task;
                var like = _mapper.Map<Like>(request._createLikeDTO);
                post.Likes.Add(like);
                await _repo.UpdateAsync(post);
                return await Unit.Task;
            }
        }
    }
}
