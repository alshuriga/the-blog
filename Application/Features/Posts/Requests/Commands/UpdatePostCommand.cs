
using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        public UpdatePostDto? PostDTO { get; set; }

        public class AddPostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;

            public AddPostCommandHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                var post = _mapper.Map<Post>(request.PostDTO);
                var id = _repo.CreateAsync(post);

                return Unit.Task;
            }
        }
    }
}
