
using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class CreatePostCommand : IRequest<long>
    {
        public CreatePostDTO? PostDTO{  get; set; }

        public class AddPostCommandHandler : IRequestHandler<CreatePostCommand, long>
        {
            private readonly IBlogRepository<Post> _repo;
            private readonly IMapper _mapper;

            public AddPostCommandHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public Task<long> Handle(CreatePostCommand request, CancellationToken cancellationToken)
            {
                var post = _mapper.Map<Post>(request.PostDTO);
                var id = _repo.CreateAsync(post);
                return id;
            }
        }
    }
}   
