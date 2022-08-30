using Ardalis.Specification;
using MiniBlog.Core.Entities;

public class PostsByTagSpecification : Specification<Post>
{
   public PostsByTagSpecification(Tag? tag, bool eager = false, bool isDraft = false)
   {
        if(eager) 
        {
            Query.Include(p => p.Commentaries);
            Query.Include(p => p.Tags);
        }
        Query.Where(p => (tag == null || p.Tags.Any(t => t.Name == tag.Name)) && p.IsDraft == isDraft);
   }
}