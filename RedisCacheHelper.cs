using System;
using Microsoft.Extensions.Caching.Distributed;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            DistributedCache.Set(cacheKey, Serialize(cacheValue),  new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expireInSeconds)
            });
        }
        public static byte[] Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            using (MemoryStream objMemoryStream = new MemoryStream())
            {
                objBinaryFormatter.Serialize(objMemoryStream, obj);
                byte[] objDataAsByte = objMemoryStream.ToArray();
                return objDataAsByte;
            }
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            if (bytes == null)
                return default(T);

            using (MemoryStream objMemoryStream = new MemoryStream(bytes))
            {
                T result = (T) objBinaryFormatter.Deserialize(objMemoryStream);
                return result;
            }
        }
    }
}
