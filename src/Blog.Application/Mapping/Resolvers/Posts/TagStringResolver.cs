using AutoMapper;
using Blog.Application.Features.Posts.DTO.Common;
using Blog.Application.Features.Tags.Specifications;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities;

namespace Blog.Application.Mapping.Resolvers.Posts
{
    public class TagToTagStringResolver : IValueResolver<Post, WritablePostDTO, string?>
    {
        public string Resolve(Post source, WritablePostDTO destination, string? destMember, ResolutionContext context)
        {
            return string.Join(",", source.Tags.Select(t => t.Name));
        }
    }
    public class TagStringToTagsResolver : IValueResolver<WritablePostDTO, Post, ICollection<Tag>>
    {
        private readonly IBlogRepository<Tag> _repo;
        public TagStringToTagsResolver(IBlogRepository<Tag> repo)
        {
            _repo = repo;
        }
        public ICollection<Tag> Resolve(WritablePostDTO source, Post destination, ICollection<Tag> destMember, ResolutionContext context)
        {
            var outputList = new List<Tag>();

            var tagNamesInput = string.IsNullOrEmpty(source.TagString) ? Enumerable.Empty<string>() : source.TagString.Split(",");

            foreach (var tn in tagNamesInput)
            {
                var tagName = tn.Trim().ToLower();
                if (string.IsNullOrWhiteSpace(tagName) || outputList.Any(t => t.Name == tagName)) continue;
                var tag = _repo.ListAsync(new TagsByTagNameSpecification(tagName)).Result.FirstOrDefault() ?? new Tag { Name = tagName.Trim().ToLower() };
                outputList.Add(tag);
            }

            return outputList;
        }
    }
}
