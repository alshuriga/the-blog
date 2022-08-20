using Ardalis.Specification;
using MiniBlog.Core.Entities;
using MiniBlog.Core.Constants;

namespace MiniBlog.Core.Specifications;

public class CommentsByPostIdSpecification : Specification<Commentary>
{
    public CommentsByPostIdSpecification(long id, long? page = null)
    {
        Query.Where(c => c.PostId == id);
        if(page != null) Query.Skip((int)(PaginationConstants.COMMENTS_PER_PAGE * (page - 1))).Take(PaginationConstants.COMMENTS_PER_PAGE);
    }
}