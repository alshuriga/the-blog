﻿
using AutoMapper;
using Blog.Application.Features.Commentaries;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blog.Application.Features.Posts.Requests.Commands
{
    public class DeleteCommentaryCommand : IRequest<Unit>
    {
        public long CommentaryId { get; set; }

        public class DeleteCommentaryCommandHandler : IRequestHandler<DeleteCommentaryCommand, Unit>
        {
            private readonly IBlogRepository<Commentary> _repo;

            public DeleteCommentaryCommandHandler(IBlogRepository<Commentary> repo, IMapper mapper, IUserService userService)
            {
                _repo = repo;
            }
            public async Task<Unit> Handle(DeleteCommentaryCommand request, CancellationToken cancellationToken)
            {
                await _repo.DeleteAsync(request.CommentaryId);
                return await Unit.Task;
            }
        }
    }
}
