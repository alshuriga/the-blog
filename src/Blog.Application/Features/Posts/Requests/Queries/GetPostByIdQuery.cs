using AutoMapper;
using Blog.Application.Constants;
using Blog.Application.Exceptions;
using Blog.Application.Features.Commentaries;
using Blog.Application.Features.Commentaries.Specifications;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Features.Posts.ViewModels;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;
using MediatR;

namespace Blog.Application.Features.Posts.Requests.Queries
{
    public class GetPostByIdQuery : IRequest<PostSingleVM>
    {
        private readonly long _id;
        private readonly int _currentPage;
        private readonly bool _includeDrafts;

        public GetPostByIdQuery(long id, int currentPage, bool includeDrafts = false)
        {
            _id = id;
            _currentPage = currentPage;
            _includeDrafts = includeDrafts;
        }

        public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostSingleVM>
        {
            private readonly IBlogRepository<Post> _postRepo;
            private readonly IBlogRepository<Commentary> _commentRepo;
            private readonly IMapper _mapper;
            public GetPostByIdQueryHandler(IBlogRepository<Post> postRepo, IBlogRepository<Commentary> commentRepo, IMapper mapper)
            {
                _postRepo = postRepo;
                _commentRepo = commentRepo;
                _mapper = mapper;
            }
            public async Task<PostSingleVM> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
            {
                Post? post = await _postRepo.GetByIdAsync(request._id);
                if (post == null || (post.IsDraft && !request._includeDrafts)) throw new NotFoundException();
                PostDTO postDto = _mapper.Map<PostDTO>(post);
                var commentaries = (await _commentRepo.ListAsync(new CommentariesByPostIdSpecification(request._id, request._currentPage))).OrderByDescending(x => x.DateTime).ToList();
                var model = new PostSingleVM()
                {
                    Post = postDto,
                    CurrentPage = request._currentPage,
                    Commentaries = _mapper.Map<List<CommentaryDTO>>(commentaries),
                    PageCount = (int)Math.Ceiling((await _commentRepo.CountAsync(new CommentariesByPostIdSpecification(request._id))) / ((double)PaginationConstants.COMMENTARIES_PER_PAGE))
                };
                return model;
            }
        }
    }
}
