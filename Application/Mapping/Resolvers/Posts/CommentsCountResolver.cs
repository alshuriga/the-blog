using AutoMapper;
using Blog.Application.Features.Posts.DTO;
using Blog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Mapping.Resolvers.Posts
{
    public class CommentsCountResolver : IValueResolver<Post, PostListDTO, int>
    {
        public int Resolve(Post source, PostListDTO destination, int destMember, ResolutionContext context)
        {
            return source.Commentaries.Count;
        }
    }
}
