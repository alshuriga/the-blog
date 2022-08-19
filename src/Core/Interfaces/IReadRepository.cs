using Ardalis.Specification;
namespace MiniBlog.Core.Interfaces;

public interface IReadRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAsync();

    Task<IEnumerable<T>> ListAsync(ISpecification<T> specification);

    Task<bool> AnyAsync(ISpecification<T> specification);

    Task<int> CountAsync();
    
    Task<int> CountAsync(ISpecification<T> specification);

}