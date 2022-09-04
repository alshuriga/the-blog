using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using MediatR;
using Blog.Core.Entities;
using AutoMapper;
using Blog.Application.Features.Posts.ViewModels;
using Blog.Application.Exceptions;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Commentaries.Specifications;
using Blog.Application.Constants;

namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class GetPostByIdQuery : IRequest<PostSingleVM>
    {
        public long Id { get; set; }
        public int CurrentPage { get; set; }

        public class GetSinglePostQueryHandler : IRequestHandler<GetPostByIdQuery, PostSingleVM>
        {
            private readonly IBlogRepository<Post> _postRepo;
            private readonly IBlogRepository<Commentary> _commentRepo;
            private readonly IMapper _mapper;
            public GetSinglePostQueryHandler(IBlogRepository<Post> postRepo, IBlogRepository<Commentary> commentRepo, IMapper mapper)
            {
                _postRepo = postRepo;
                _commentRepo = commentRepo;
                _mapper = mapper;
            }
            public async Task<PostSingleVM> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                Post? post =  await _postRepo.GetByIdAsync(request.Id);
                if (post == null) throw new NotFoundException();
                PostDTO postDto = _mapper.Map<PostDTO>(post);
                var commentaries = (await _commentRepo.ListAsync(new CommentariesByPostIdSpecification(request.Id, request.CurrentPage))).OrderByDescending(x => x.DateTime).ToList();
                var model = new PostSingleVM()
                {
                    Post = postDto,
                    CurrentPage = request.CurrentPage,
                    Commentaries = _mapper.Map<List<CommentaryDTO>>(commentaries),
                    PageCount = (int)Math.Ceiling((await _commentRepo.CountAsync(new CommentariesByPostIdSpecification(request.Id))) / ((double)PaginationConstants.COMMENTARIES_PER_PAGE))
                };
                return model;
            }
        }
    }
}
