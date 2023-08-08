using Ardalis.Specification;
using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Likes.Specifications;

public class LikesByUsernameSpecification : Specification<Like>
{
    public LikesByUsernameSpecification(string username)
    {
        Query.Where(l => l.Username == username);
        Query.EnableCache(nameof(LikesByUsernameSpecification), username);
    }
}
