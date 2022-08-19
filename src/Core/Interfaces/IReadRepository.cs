using Ardalis.Specification;
namespace MiniBlog.Core.Interfaces;

public interface IReadRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAsync();

    Task<IEnumerable<T>> ListAsync(ISpecification<T> specification);

    Task<T?> RetrieveByIdAsync(long id, bool eager = false);

    Task<bool> AnyAsync(ISpecification<T> specification);

    Task<int> CountAsync();
    
    Task<int> CountAsync(ISpecification<T> specification);

}