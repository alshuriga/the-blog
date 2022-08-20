using MiniBlog.Core.Entities;

namespace MiniBlog.Core.Interfaces;
    public interface ITagService
    {
        Task UpdatePostTags(Post post, IEnumerable<Tag> tags);
    }
