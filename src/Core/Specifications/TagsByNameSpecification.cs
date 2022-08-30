using Ardalis.Specification;
using MiniBlog.Core.Entities;

namespace MiniBlog.Core.Specifications;

public class TagsByPostIdSpecification : Specification<Tag>
{
    public TagsByPostIdSpecification(long postId)
    {
        Query.Include(t => t.Posts).Where(t => t.Posts!.Where(p => p.Id == postId).Any());
    }
}