using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using ServicesAbstraction;

namespace Services.Cache
{
    public class CacheService(IDistributedCache _distributedCache) : ICacheService
    {
        public async Task<string?> GetAsync(string key)
        {
            var res = await _distributedCache.GetAsync(key);
            return res is null ? null : Encoding.UTF8.GetString(res);
        }

        public async Task SetAsync(string key, string value, TimeSpan duration)
        {
            var res = Encoding.UTF8.GetBytes(value);
            await _distributedCache.SetAsync(key,res, new DistributedCacheEntryOptions
            {
               AbsoluteExpirationRelativeToNow = duration
            });
        }
    }
}