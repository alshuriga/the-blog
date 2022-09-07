
using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class CreatePostCommand : IRequest<long>
    {
        private readonly CreatePostDTO _postDTO;

        public CreatePostCommand(CreatePostDTO postDTO)
        {
            _postDTO = postDTO;
        }

        public class AddPostCommandHandler : IRequestHandler<CreatePostCommand, long>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;
            private readonly IValidator<CreatePostDTO> _validator;

            public AddPostCommandHandler(IBlogRepository<Post> repo, IMapper mapper, IValidator<CreatePostDTO> validator)
            {
                _repo = repo;
                _mapper = mapper;
                _validator = validator;
            }
            public Task<long> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request._postDTO);
                var post = _mapper.Map<Post>(request._postDTO);
                var id = _repo.CreateAsync(post);
                return id;
            }
        }
    }
}   
