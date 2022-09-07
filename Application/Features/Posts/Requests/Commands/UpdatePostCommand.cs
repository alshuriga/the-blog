
using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        private readonly UpdatePostDTO _postDTO;

        public UpdatePostCommand(UpdatePostDTO postDTO)
        {
            _postDTO = postDTO;
        }

        public class AddPostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;
            private readonly IValidator<UpdatePostDTO> _validator;


            public AddPostCommandHandler(IBlogRepository<Post> repo, IMapper mapper, IValidator<UpdatePostDTO> validator)
            {
                _repo = repo;
                _mapper = mapper;
                _validator = validator;
            }
            public Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request._postDTO);
                var post = _mapper.Map<Post>(request._postDTO);
                var id = _repo.UpdateAsync(post);

                return Unit.Task;
            }
        }
    }
}
