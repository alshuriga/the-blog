using AutoMapper;
using Blog.Application.Features.Posts.DTO.Common;
using Blog.Application.Features.Tags.Specifications;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;

namespace Blog.Application.Mapping.Resolvers.Posts
{
    public class TagToTagStringResolver : IValueResolver<Post, IPostDTO, string>
    {
        public string Resolve(Post source, IPostDTO destination, string destMember, ResolutionContext context)
        {
            return string.Join(",", source.Tags.Select(t => t.Name));
        }
    }
    public class TagStringToTagsResolver : IValueResolver<IPostDTO, Post, ICollection<Tag>>
    {
        private readonly IBlogRepository<Tag> _repo;
        public TagStringToTagsResolver(IBlogRepository<Tag> repo)
        {
            _repo = repo;
        }
        public ICollection<Tag> Resolve(IPostDTO source, Post destination, ICollection<Tag> destMember, ResolutionContext context)
        {
            var outputList = new List<Tag>();
            var tagNamesInput = string.IsNullOrEmpty(source.TagString) ? Enumerable.Empty<string>() : source.TagString.Split(",");
            foreach (var tagName in tagNamesInput)
            {
                var tag = _repo.ListAsync(new TagsByTagNameSpecification(tagName)).Result.FirstOrDefault() ?? new Tag { Name = tagName.Trim().ToLower() };
                outputList.Add(tag);
            }
            return outputList;
        }
    }
}
