using Ardalis.Specification;
using Blog.Application.Constants;
using Blog.Core.Entities;

namespace Blog.Application.Features.Commentaries.Specifications
{
    public class CommentariesByPostIdSpecification : Specification<Commentary>
    {
        public CommentariesByPostIdSpecification(long postId, int? currentPage = null)
        {
            Query.Where(c => c.PostId == postId);
            if (currentPage != null)
            {
                if (currentPage > 0) Query.Skip(PaginationConstants.COMMENTARIES_PER_PAGE * (int)currentPage);
                Query.Take(PaginationConstants.COMMENTARIES_PER_PAGE);
            }
        }
    }
}
