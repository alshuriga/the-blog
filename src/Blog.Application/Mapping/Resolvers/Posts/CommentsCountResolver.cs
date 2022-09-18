using AutoMapper;
using Blog.Application.Features.Commentaries.Specifications;
using Blog.Application.Features.Posts.DTO;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;


namespace Blog.Application.Mapping.Resolvers.Posts
{
    public class CommentsCountResolver : IValueResolver<Post, PostListVM, int>
    {
        private readonly IBlogRepository<Commentary> _commentRepo;
        public CommentsCountResolver(IBlogRepository<Commentary> commentRepo)
        {
            _commentRepo = commentRepo;
        }

        public int Resolve(Post source, PostListVM destination, int destMember, ResolutionContext context)
        {
            return _commentRepo.CountAsync(new CommentariesByPostIdSpecification(source.Id)).Result;
        }
    }
}
