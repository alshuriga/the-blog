using AutoMapper;
using Blog.Application.Features.Likes.Specifications;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;
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
            private readonly IBlogRepository<Like> _repo;
            private readonly IMapper _mapper;
            IValidator<CreateDeleteLikeDTO> _validator;

            public LikePostCommandHandler(IBlogRepository<Like> repo, IMapper mapper, IValidator<CreateDeleteLikeDTO> validator)
            {
                _repo = repo;
                _mapper = mapper;
                _validator = validator; 
            }   

            public async Task<Unit> Handle(LikePostCommand request, CancellationToken cancellationToken)
            {
                var likes = await _repo.ListAsync(new LikesByUsernameSpecification(request._createLikeDTO.Username));
                if (likes.Any(l => l.PostId == request._createLikeDTO.PostId)) return await Unit.Task;
                var like = _mapper.Map<Like>(request._createLikeDTO);
                await _repo.CreateAsync(like);
                return await Unit.Task;
            }
        }
    }
}
