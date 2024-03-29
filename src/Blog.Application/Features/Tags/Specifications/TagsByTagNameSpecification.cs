﻿using Ardalis.Specification;
using Blog.Core.Entities;

namespace Blog.Application.Features.Tags.Specifications
{
    public class TagsByTagNameSpecification : Specification<Tag>
    {

        public TagsByTagNameSpecification(string tagName)
        {
            Query.Where(t => t.Name.Trim().ToLower() == tagName.Trim().ToLower());
            Query.EnableCache(nameof(TagsByTagNameSpecification), tagName);
        }
    }
}
