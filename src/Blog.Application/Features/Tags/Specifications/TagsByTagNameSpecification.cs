using Ardalis.Specification;
using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Tags.Specifications
{
    public class TagsByTagNameSpecification : Specification<Tag>
    {
        
        public TagsByTagNameSpecification(string tagName)
        {
            Query.Where(t => t.Name == tagName);

            Query.EnableCache(nameof(TagsByTagNameSpecification), tagName);
        }
    }
}
