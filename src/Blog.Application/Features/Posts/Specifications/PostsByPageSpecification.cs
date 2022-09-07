using Ardalis.Specification;
using Blog.Core.Entities;
using Blog.Application.Constants; 

namespace Blog.Application.Features.Posts.Specifications
{
    public class PostsSpecification : Specification<Post>
    {
        public PostsSpecification(int? currentPage = null, string? tagName = null, bool? isDraft = null)
        {
            Query.Where(p => (tagName == null || p.Tags.Any(t => t.Name == tagName))
                    && (isDraft == null || p.IsDraft == isDraft));
            if (currentPage != null)
            {
                if (currentPage > 0) Query.Skip(PaginationConstants.POSTS_PER_PAGE * (int)currentPage);
                Query.Take(PaginationConstants.POSTS_PER_PAGE);
            }
        }
    }
}
