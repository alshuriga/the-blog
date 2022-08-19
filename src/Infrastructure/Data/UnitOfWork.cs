using MiniBlog.Core.Entities;
using MiniBlog.Core.Interfaces;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly IRepository<Post> _postRepo;
    private readonly IReadRepository<Post> _postReadRepo;
    private readonly IRepository<Commentary> _commentRepo;
    private readonly IReadRepository<Commentary> _commentReadRepo;
    private readonly IRepository<Tag> _tagRepo;
    private readonly IReadRepository<Tag> _tagReadRepo;

    public EfUnitOfWork(IRepository<Post> postsRepo,
        IReadRepository<Post> postsReadRepo, 
        IRepository<Commentary> commentsRepo,
        IReadRepository<Commentary> commentsReadRepo,
        IReadRepository<Tag> tagsReadRepo,
        IRepository<Tag> tagsRepo)
    {
        _postRepo = postsRepo;
        _postReadRepo = postsReadRepo;
        _commentRepo = commentsRepo;
        _commentReadRepo = commentsReadRepo;
        _tagReadRepo = tagsReadRepo;
        _tagRepo = tagsRepo;
    }

    public IRepository<Post> postRepo => _postRepo;

    public IReadRepository<Post> postReadRepo => _postReadRepo;

    public IRepository<Commentary> commentRepo => _commentRepo;

    public IReadRepository<Commentary> commentReadRepo => _commentReadRepo;

    public IRepository<Tag> tagsRepo => _tagRepo;

    public IReadRepository<Tag> tagsReadRepo => _tagReadRepo;
}