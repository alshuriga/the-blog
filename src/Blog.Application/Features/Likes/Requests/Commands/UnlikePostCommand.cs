using AutoMapper;
using Blog.Application.Features.Likes.Specifications;
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
            private readonly IBlogRepository<Like> _repo;
            private readonly IMapper _mapper;

            public UnlikePostCommandHandler(IBlogRepository<Like> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }   

            public async Task<Unit> Handle(UnlikePostCommand request, CancellationToken cancellationToken)
            {
                var userlikes = await _repo.ListAsync(new LikesByUsernameSpecification(request._createLikeDTO.Username));
                var like = userlikes.FirstOrDefault(l => l.PostId == request._createLikeDTO.PostId);
                if(like == null) return await Unit.Task;
                await _repo.DeleteAsync(like.Id);
                return await Unit.Task;
            }
        }
    }
}
