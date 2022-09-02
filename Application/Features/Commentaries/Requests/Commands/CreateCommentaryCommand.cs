
using AutoMapper;
using Blog.Application.Features.Commentaries;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class CreateCommentaryCommand : IRequest<long>
    {
        public CreateCommentaryDTO? CommentaryDTO { get; set; }
        public long PostId { get; set; }

        public class AddPostCommandHandler : IRequestHandler<CreateCommentaryCommand, long>
        {
            private readonly IBlogRepository<Commentary> _repo;
            private readonly IMapper _mapper;

            public AddPostCommandHandler(IBlogRepository<Commentary> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public Task<long> Handle(CreateCommentaryCommand request, CancellationToken cancellationToken)
            {
                var commentary = _mapper.Map<Commentary>(request.CommentaryDTO);
                commentary.PostId = request.PostId;
                var id = _repo.CreateAsync(commentary);
                return id;
            }
        }
    }
}
