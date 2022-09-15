using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities.Common;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Infrastructure.Data
{
    public class CachedBlogRepository<T> : IBlogRepository<T> where T : BaseEntity
    {

        private readonly EFBlogRepository<T> _repo;
        private readonly IMemoryCache _cache;
        private string _keyPrefix;
        private MemoryCacheEntryOptions _cacheOptions;

        public CachedBlogRepository(EFBlogRepository<T> repo, IMemoryCache cache)
        {
            _repo = repo;
            _cache = cache;
            _cacheOptions = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(1)
            };
            _keyPrefix = typeof(T).ToString();
        }

        public async Task<int> CountAsync(ISpecification<T>? specification = null)
        {
            var key = GetCountKey(specification);
            return await _cache.GetOrCreate(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _repo.CountAsync(specification);
            });
        }

        public async Task<long> CreateAsync(T entity)
        {
            var result =  await _repo.CreateAsync(entity);
            var key = GetListKey(null);
            _cache.Remove(key);
            return result;

        }

        public async Task DeleteAsync(long id)
        {
            await _repo.DeleteAsync(id);
            var key = GetSingleEntryKey(id);
            _cache.Remove(key);
        }

        public async Task<T?> GetByIdAsync(long Id)
        {
            var key = GetSingleEntryKey(Id);
            return await _cache.GetOrCreateAsync(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _repo.GetByIdAsync(Id);
            });

        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null)
        {
            var key = GetListKey(specification);
            var result = _cache.GetOrCreate(key, async entry =>
            {
                entry.SetOptions(_cacheOptions);
                return await _repo.ListAsync(specification);
            });
            return await result;
        }

        public async Task UpdateAsync(T entity)
        {
            var key = GetSingleEntryKey(entity.Id);
            await _repo.UpdateAsync(entity);
            _cache.Remove(key);
        }

        private string GetListKey(ISpecification<T>? specification) => $"{_keyPrefix}-{specification?.CacheKey}-list";
        private string GetSingleEntryKey(long id) => $"{_keyPrefix}-{id}-single";
        private string GetCountKey(ISpecification<T>? specification) => $"{_keyPrefix}-{specification?.CacheKey}-count";
    }
}
