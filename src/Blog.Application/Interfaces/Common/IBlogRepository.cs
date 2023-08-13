using Ardalis.Specification;
using Blog.Core.Entities.Common;

namespace Blog.Application.Interfaces.Common;

public interface IBlogRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(long Id, ISpecification<T>? specification = null);
    Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null);
    Task<long> CreateAsync(T entity);
    Task DeleteAsync(long Id);
    Task UpdateAsync(T entity);
    Task<int> CountAsync(ISpecification<T>? specification = null);
}
