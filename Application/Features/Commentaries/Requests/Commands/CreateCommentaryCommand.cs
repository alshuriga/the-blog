
using AutoMapper;
using Blog.Application.Features.Commentaries;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class CreateCommentaryCommand : IRequest<long>
    {
        public CreateCommentaryDTO CommentaryDTO { get; set; } = null!;
        public long PostId { get; set; }
        public string Username { get; set; } = null!;

        public class AddPostCommandHandler : IRequestHandler<CreateCommentaryCommand, long>
        {
            private readonly IBlogRepository<Commentary> _repo;
            private readonly IMapper _mapper;
            private readonly IUserService _userService;
            private readonly IValidator<CreateCommentaryDTO> _validator;

            public AddPostCommandHandler(IBlogRepository<Commentary> repo, IMapper mapper, IUserService userService, IValidator<CreateCommentaryDTO> validator)
            {
                _repo = repo;
                _mapper = mapper;
                _userService = userService;
                _validator = validator;
            }
            public async Task<long> Handle(CreateCommentaryCommand request, CancellationToken cancellationToken)
            {
                _validator.ValidateAndThrow(request.CommentaryDTO);
                var commentary = _mapper.Map<Commentary>(request.CommentaryDTO);
                var userData = await _userService.GetUserByNameAsync(request.Username);
                commentary.PostId = request.PostId;
                commentary.Username =userData.Username;
                commentary.Email = userData.Email;
                var id = await _repo.CreateAsync(commentary);
                return id;
            }
        }
    }
}
