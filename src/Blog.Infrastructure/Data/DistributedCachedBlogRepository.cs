
using Ardalis.Specification;
using Blog.Application.Interfaces.Common;
using Blog.Core.Entities.Common;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Blog.Infrastructure.Data
{
    public class DistributedCachedBlogRepository<T> : IBlogRepository<T> where T : BaseEntity
    {

        private readonly EFBlogRepository<T> _repo;
        private readonly IDistributedCache _cache;
        private string _keyPrefix;
        private DistributedCacheEntryOptions _cacheOptions;

        public DistributedCachedBlogRepository(EFBlogRepository<T> repo, IDistributedCache cache)
        {
            _repo = repo;
            _cache = cache;
            _cacheOptions = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5)
            };
            _keyPrefix = typeof(T).ToString();
        }

        public async Task<int> CountAsync(ISpecification<T>? specification = null)
        {
            var key = GetCountKey(specification);
            var result = await _cache.GetOrCreateAsync(key, async() => 
            await _repo.CountAsync(specification)
            , _cacheOptions);
            return result;
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
            return await _cache.GetOrCreateAsync(key, async () =>
            {
                return await _repo.GetByIdAsync(Id);
            }, _cacheOptions);

        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T>? specification = null)
        {
            var key = GetListKey(specification);
            var result = _cache.GetOrCreateAsync(key, async () =>
            {
                return await _repo.ListAsync(specification);
            }, _cacheOptions);
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
