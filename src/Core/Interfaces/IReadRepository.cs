using Ardalis.Specification;
using System.Linq.Expressions;

namespace MiniBlog.Core.Interfaces;

public interface IReadRepository<T> where T : BaseEntity
{
    Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null);

    Task<T?> RetrieveByIdAsync(long id, Expression<Func<T, object>>[]? includes = null);

    Task<bool> AnyAsync(ISpecification<T>? specification = null);
    
    Task<int> CountAsync(ISpecification<T>? specification = null);

}