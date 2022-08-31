using Ardalis.Specification;
using Blog.Core.Entities;
using Blog.Application.Constants; 

namespace Blog.Application.Features.Posts.Specifications
{
    public class PostsByPageSpecification : Specification<Post>
    {
        public PostsByPageSpecification(int currentPage, string? tagName = null)
        {
            if (tagName != null) Query.Where(p => p.Tags.Any(t => t.Name == tagName));
            if (currentPage > 0) Query.Skip(PaginationConstants.POSTS_PER_PAGE * currentPage);
            Query.Take(PaginationConstants.POSTS_PER_PAGE);
        }
    }
}
