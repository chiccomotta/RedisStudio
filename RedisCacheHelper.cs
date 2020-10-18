using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Extensions.Caching.Distributed;

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
            return Deserialize<T>(DistributedCache.Get(cacheKey));
        }

        public void Set(string cacheKey, object cacheValue, int expireInSeconds)
        {
            DistributedCache.Set(cacheKey, Serialize(cacheValue), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireInSeconds)
            });
        }

        private static byte[] Serialize(object obj)
        {
            if (obj == null) return null;

            var objBinaryFormatter = new BinaryFormatter();
            using (var objMemoryStream = new MemoryStream())
            {
                objBinaryFormatter.Serialize(objMemoryStream, obj);
                return objMemoryStream.ToArray();
            }
        }

        private static T Deserialize<T>(byte[] bytes)
        {
            var objBinaryFormatter = new BinaryFormatter();
            if (bytes == null)
                return default;

            using (var objMemoryStream = new MemoryStream(bytes))
            {
                return (T) objBinaryFormatter.Deserialize(objMemoryStream);
            }
        }
    }
}