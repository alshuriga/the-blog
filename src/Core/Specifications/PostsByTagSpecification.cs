using Ardalis.Specification;
using MiniBlog.Core.Entities;

public class PostsByTagSpecification : Specification<Post>
{
   public PostsByTagSpecification(Tag tag, bool eager = false)
   {
        if(eager) 
        {
            Query.Include(p => p.Commentaries);
            Query.Include(p => p.Tags);
        }
        Query.Where(p => p.Tags.Where(t => t.Name == tag.Name).Any());
   }
}