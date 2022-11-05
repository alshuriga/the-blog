using Newtonsoft.Json;
using StackExchange.Redis;

namespace Microsoft.Extensions.Caching.Distributed;

public static class DistributedCacheExtensions
{
    public static async Task<TEntry> GetOrCreateAsync<TEntry>(this IDistributedCache cache, string key, Func<Task<TEntry>> factory, DistributedCacheEntryOptions? options = null)
    {
        TEntry? result;
        try
        {
            var cacheResult = await cache.GetStringAsync(key);
            
            if (cacheResult != null)
            {
                result = JsonConvert.DeserializeObject<TEntry>(cacheResult);
                if (result != null)
                {
                    return result;
                }
            }
        }
        catch (RedisTimeoutException ex)
        {  
            Console.WriteLine(ex.Message);
        }

        result = await factory.Invoke();
        await cache.SetStringAsync(key, JsonConvert.SerializeObject(result, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        }), options ?? new());

        return result;
    }
}
