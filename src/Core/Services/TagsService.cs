
using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;
using MiniBlog.Core.Specifications;

public class TagService : ITagService
{
    private readonly IRepository<Tag> _tagRepo;
    private readonly IReadRepository<Tag> _tagReadRepo;
    public TagService(IRepository<Tag> tagRepo, IReadRepository<Tag> tagReadRepo)
    {
        _tagRepo = tagRepo;
        _tagReadRepo = tagReadRepo;
    }

    public async Task UpdatePostTags(Post post, IEnumerable<Tag> tags)
    {
        var existingTags = await _tagReadRepo.ListAsync(new TagsByPostIdSpecification(post.Id));
        foreach(var tag in existingTags)
        {
            tag.Posts.Remove(post);
        }
        foreach(var tag in tags)
        {
            post.Tags.Add(tag);
        }
       
    }
}