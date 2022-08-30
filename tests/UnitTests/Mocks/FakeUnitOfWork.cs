
//using Ardalis.Specification;
//using MiniBlog.Core.Entities;
//using MiniBlog.Core.Interfaces;

//namespace MiniBlog.Tests.UnitTests.Mocks;

//public class FakeUnitOfWork : IUnitOfWork
//{
//    private readonly IRepository<Post> _postRepo;
//    private readonly IReadRepository<Post> _postReadRepo;
//    private readonly IRepository<Commentary> _commentRepo;
//    private readonly IReadRepository<Commentary> _commentReadRepo;
//    private readonly IRepository<Tag> _tagRepo;
//    private readonly IReadRepository<Tag> _tagReadRepo;

//    public FakeUnitOfWork(
//        IRepository<Post> postsRepo,
//        IReadRepository<Post> postsReadRepo,
//        IRepository<Commentary> commentsRepo,
//        IReadRepository<Commentary> commentsReadRepo,
//        IReadRepository<Tag> tagsReadRepo,
//        IRepository<Tag> tagsRepo
//        )
//    {
//        _postRepo = postsRepo;
//        _postReadRepo = postsReadRepo;
//        _commentRepo = commentsRepo;
//        _commentReadRepo = commentsReadRepo;
//        _tagReadRepo = tagsReadRepo;
//        _tagRepo = tagsRepo;
//    }

//    public IRepository<Post> postRepo => _postRepo;

//    public IReadRepository<Post> postReadRepo => _postReadRepo;

//    public IRepository<Commentary> commentRepo => _commentRepo;

//    public IReadRepository<Commentary> commentReadRepo => _commentReadRepo;

//    public IRepository<Tag> tagsRepo => _tagRepo;

//    public IReadRepository<Tag> tagsReadRepo => _tagReadRepo;
//}

//public class FakeTagReadRepo : IReadRepository<Tag>
//{
//    private readonly SeedTestData _db = new SeedTestData();

//    public Task<bool> AnyAsync(ISpecification<Tag> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Tags).Any());
//    }

//    public Task<int> CountAsync()
//    {
//        return Task.FromResult(SeedTestData.Posts.Count());
//    }

//    public Task<int> CountAsync(ISpecification<Tag> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Tags).Count());
//    }

//    public Task<IEnumerable<Tag>> ListAsync()
//    {
//        return Task.FromResult(SeedTestData.Tags);
//    }

//    public Task<IEnumerable<Tag>> ListAsync(ISpecification<Tag> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Tags));
//    }

//    public Task<Tag?> RetrieveByIdAsync(long id, bool eager = false)
//    {
//        var res = SeedTestData.Tags.FirstOrDefault(p => p.Id == id);
//        if (!eager)
//        {
//            res!.Posts = new List<Post>();
//        }
//        return Task.FromResult(res);
//    }
//}

//public class FakePostReadRepo : IReadRepository<Post>
//{
//    public Task<bool> AnyAsync(ISpecification<Post> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Posts).Any());
//    }

//    public Task<int> CountAsync()
//    {
//        return Task.FromResult(SeedTestData.Posts.Count());
//    }

//    public Task<int> CountAsync(ISpecification<Post> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Posts).Count());
//    }

//    public Task<IEnumerable<Post>> ListAsync()
//    {
//        return Task.FromResult(SeedTestData.Posts);
//    }

//    public Task<IEnumerable<Post>> ListAsync(ISpecification<Post> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Posts));
//    }

//    public Task<Post?> RetrieveByIdAsync(long id, bool eager = false)
//    {
//        var res = SeedTestData.Posts.FirstOrDefault(p => p.Id == id);
//        if (!eager)
//        {
//            res!.Tags = new List<Tag>();
//            res.Commentaries = new List<Commentary>();
//        }
//        return Task.FromResult(res);
//    }
//}

//public class FakeCommentaryReadRepo : IReadRepository<Commentary>
//{
//    private readonly SeedTestData _db = new SeedTestData();

//    public Task<bool> AnyAsync(ISpecification<Commentary> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Commentaries).Any());
//    }

//    public Task<int> CountAsync()
//    {
//        return Task.FromResult(SeedTestData.Commentaries.Count());
//    }

//    public Task<int> CountAsync(ISpecification<Commentary> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Commentaries).Count());
//    }

//    public Task<IEnumerable<Commentary>> ListAsync()
//    {
//        return Task.FromResult(SeedTestData.Commentaries);
//    }

//    public Task<IEnumerable<Commentary>> ListAsync(ISpecification<Commentary> specification)
//    {
//        return Task.FromResult(specification.Evaluate(SeedTestData.Commentaries));
//    }

//    public Task<Commentary?> RetrieveByIdAsync(long id, bool eager = false)
//    {
//        var res = SeedTestData.Commentaries.FirstOrDefault(p => p.Id == id);
//        if (!eager)
//        {
//            res!.Post = null;
//        }
//        return Task.FromResult(res);
//    }
//}


