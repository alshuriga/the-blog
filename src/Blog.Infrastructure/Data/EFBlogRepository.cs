using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure.Data
{
    public class EFBlogRepository<T> : IBlogRepository<T> where T : BaseEntity
    {

        private readonly BlogEFContext _db;

        public EFBlogRepository(BlogEFContext db)
        {
            _db = db;
        }

        public Task<int> CountAsync(ISpecification<T>? specification = null)
        {
            var query = _db.Set<T>().AsQueryable();

            if (specification != null)
                query = query.WithSpecification(specification);

            return Task.FromResult(query.Count());
        }

        public async Task<long> CreateAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(long Id)
        {
            T? entity = await _db.Set<T>().FindAsync(Id);
            if (entity != null)
            {
                _db.Set<T>().Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<T?> GetByIdAsync(long Id)
        {
            return await _db.Set<T>().FindAsync(Id);
        }

        public Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null)
        {
            var query = _db.Set<T>().OrderByDescending(e => e.Id).AsQueryable();
            if (specification != null) query = query.WithSpecification(specification);
            return Task.FromResult(query.AsEnumerable());
        }

        public async Task UpdateAsync(T entity)
        {
            _db.Set<T>().Update(entity);
            await _db.SaveChangesAsync();
        }
    }
}
