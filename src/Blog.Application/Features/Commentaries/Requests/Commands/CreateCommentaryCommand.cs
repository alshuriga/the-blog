
using AutoMapper;
using Blog.Application.Features.Commentaries;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using FluentValidation;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class CreateCommentaryCommand : IRequest<long>
    {
        private readonly CreateCommentaryDTO _commentaryDTO;
        private readonly long _postId;
        private readonly string _username;

        public CreateCommentaryCommand(CreateCommentaryDTO commentaryDTO, long postId, string username)
        {
            _commentaryDTO = commentaryDTO;
            _postId = postId;
            _username = username;
        }

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
                await _validator.ValidateAndThrowAsync(request._commentaryDTO);
                var commentary = _mapper.Map<Commentary>(request._commentaryDTO);
                var userData = await _userService.GetUserByNameAsync(request._username);
                commentary.PostId = request._postId;
                commentary.Username = userData!.Username;
                commentary.Email = userData.Email;
                var id = await _repo.CreateAsync(commentary);
                return id;
            }
        }
    }
}
