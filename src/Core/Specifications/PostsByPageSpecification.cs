using MiniBlog.Core.Entities;
using MiniBlog.Constants;
using Ardalis.Specification;
public class PostsByPageSpecification : Specification<Post>
{
    public PostsByPageSpecification(int currentPage, Tag? tag = null, bool eager = false)
    {
        if(eager) Query.Include(p => p.Commentaries).Include(p => p.Tags);
       
        Query.Where(p => tag == null || p.Tags.Any(t => t.Name == tag.Name));
         
        if(currentPage > 1) Query.Skip(PaginationConstants.POSTS_PER_PAGE * (currentPage - 1));

        Query.Take(PaginationConstants.POSTS_PER_PAGE);
    }
}