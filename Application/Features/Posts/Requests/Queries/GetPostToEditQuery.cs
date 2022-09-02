using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class GetPostToEditQuery : IRequest<UpdatePostDto>
    {
        public long Id { get; set;  }

        public class GetPostToEditQueryHandler : IRequestHandler<GetPostToEditQuery, UpdatePostDto>
        {
            private readonly IBlogRepository<Blog.Core.Entities.Post> _repo;
            private readonly IMapper _mapper;
            public GetPostToEditQueryHandler(IBlogRepository<Post> repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<UpdatePostDto> Handle(GetPostToEditQuery request, CancellationToken cancellationToken)
            {
                var post = await _repo.GetByIdAsync(request.Id);
                var result = _mapper.Map<UpdatePostDto>(post);
                return result;
            }
        }
    }
}
