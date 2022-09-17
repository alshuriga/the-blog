

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensionMethods
{

    public static async Task<TItem> GetOrCreateAsync<TItem>(this IDistributedCache cache, string key, Func<Task<TItem>> factory)
    {
        object result = cache.GetString(key);
        if (string.IsNullOrEmpty(result.ToString()))
        {
            var entry = factory.Invoke();
            cache.SetString(key, entry!.ToString());
            return await entry;
        }
        return await Task.FromResult((TItem)result);
    }
}

