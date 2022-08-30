using Ardalis.Specification;
using MiniBlog.Core.Entities;

namespace MiniBlog.Core.Specifications;

public class TagsByNameSpecification : Specification<Tag>
{
    public TagsByNameSpecification(string name)
    {
        Query.Where(t => t.Name.ToLower() == name.ToLower());
    }
}