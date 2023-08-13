using Ardalis.Specification;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Features.Posts.Specifications;

public class PostWithCommentariesSpecification : Specification<Post>
{
    public PostWithCommentariesSpecification(int commentariesPerPage, int pageNumber)
    {
        Query.Include(p => p.Commentaries.OrderByDescending(c => c.DateTime).Skip(commentariesPerPage * pageNumber).Take(commentariesPerPage));
        Query.EnableCache(nameof(PostWithCommentariesSpecification), commentariesPerPage, pageNumber);
    }
}
