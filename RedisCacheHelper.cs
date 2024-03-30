using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;

namespace RedisStudio
{
    public class RedisCacheHelper
    {
        private readonly IDistributedCache DistributedCache;

        public RedisCacheHelper(IDistributedCache distributedCache)
        {
            DistributedCache = distributedCache;
        }

        public T Get<T>(string cacheKey)
        {
            return Deserialize<T>(DistributedCache.GetString(cacheKey));
        }

        public void Set(string cacheKey, object cacheValue, int expireInSeconds)
        {
            DistributedCache.SetString(cacheKey, Serialize(cacheValue), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireInSeconds)
            });
        }

        private static string Serialize(object obj)
        {
            if (obj == null) return null;

            return JsonConvert.SerializeObject(obj);
        }

        private static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}