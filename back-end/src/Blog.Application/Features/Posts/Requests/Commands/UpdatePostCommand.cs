
using AutoMapper;
using Blog.Application.Exceptions;
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
            public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request._postDTO);
                var post = await _repo.GetByIdAsync(request._postDTO.PostId);
                if (post == null) throw new NotFoundException();
                _mapper.Map(request._postDTO, post);
                await _repo.UpdateAsync(post);
                return await Unit.Task;
            }
        }
    }
}
