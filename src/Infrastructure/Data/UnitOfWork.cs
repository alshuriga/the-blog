using MiniBlog.Core.Entities;
using MiniBlog.Core.Interfaces;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly IRepository<Post> _postRepo;
    private readonly IReadRepository<Post> _postReadRepo;
    private readonly IRepository<Commentary> _commentRepo;
    private readonly IReadRepository<Commentary> _commentReadRepo;
    // private readonly IRepository<Tag> _tagRepo;
    // private readonly IReadRepository<Tag> _tagReadRepo;

    public EfUnitOfWork(IRepository<Post> postRepo,
        IReadRepository<Post> postReadRepo, 
        IRepository<Commentary> commentRepom,
        IReadRepository<Commentary> commentReadRepo)
    {
        _postRepo = postRepo;
        _postReadRepo = postReadRepo;
        _commentRepo = commentRepo;
        _commentReadRepo = commentReadRepo;
    }

    public IRepository<Post> postRepo => _postRepo;

    public IReadRepository<Post> postReadRepo => _postReadRepo;

    public IRepository<Commentary> commentRepo => _commentRepo;

    public IReadRepository<Commentary> commentReadRepo => _commentReadRepo;

    public IRepository<Tag> tagsRepo => throw new NotImplementedException();

    public IReadRepository<Tag> tagsReadRepo => throw new NotImplementedException();
}