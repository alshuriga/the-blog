using AutoMapper;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Likes.Requests.Commands
{
    public class UnlikePostCommand : IRequest<Unit>
    {
        private readonly CreateDeleteLikeDTO _createLikeDTO;
        

        public UnlikePostCommand(CreateDeleteLikeDTO createLikeDTO)
        {
            _createLikeDTO = createLikeDTO;
        }

        public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, Unit>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;

            public UnlikePostCommandHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }   

            public async Task<Unit> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
            {
                var post = await _repo.GetByIdAsync(request._createLikeDTO.PostId);
                if (post == null) return await Unit.Task;
                var like = post.Likes.FirstOrDefault(l => l.Username == request._createLikeDTO.Username);
                if(like == null) return await Unit.Task;
                post.Likes.Remove(like);
                await _repo.UpdateAsync(post);
                return await Unit.Task;
            }
        }
    }
}
