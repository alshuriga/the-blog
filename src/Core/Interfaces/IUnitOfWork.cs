using MiniBlog.Core.Interfaces;
using MiniBlog.Core.Entities;

public interface IUnitOfWork
{
    public IRepository<Post> postRepo { get; }
    public IReadRepository<Post> postReadRepo { get; }

    public IRepository<Commentary> commentRepo { get; }
    public IReadRepository<Commentary> commentReadRepo { get; }

    public IRepository<Tag> tagsRepo { get; }
    public IReadRepository<Tag> tagsReadRepo { get; }
}