
using AutoMapper;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class DeleteCommentaryCommand : IRequest<Unit>
    {
        private readonly long _commentaryId;

        public DeleteCommentaryCommand(long commentaryId)
        {
            _commentaryId = commentaryId;
        }

        public class DeleteCommentaryCommandHandler : IRequestHandler<DeleteCommentaryCommand, Unit>
        {
            private readonly IBlogRepository<Commentary> _repo;

            public DeleteCommentaryCommandHandler(IBlogRepository<Commentary> repo, IMapper mapper, IUserService userService)
            {
                _repo = repo;
            }
            public async Task<Unit> Handle(DeleteCommentaryCommand request, CancellationToken cancellationToken)
            {
                await _repo.DeleteAsync(request._commentaryId);
                return await Unit.Task;
            }
        }
    }
}
