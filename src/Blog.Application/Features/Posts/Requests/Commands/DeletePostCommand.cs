using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class DeletePostCommand : IRequest<Unit>
    {
        private readonly long _id;

        public DeletePostCommand(long id)
        {
            _id = id;
        }

        public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
        {
            private readonly IBlogRepository<Post> _repo;

            public DeletePostCommandHandler(IBlogRepository<Post> repo)
            {
                _repo = repo;
            }
            public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
            {
                await _repo.DeleteAsync(request._id);
                return await Unit.Task;
            }
        }
    }
}
