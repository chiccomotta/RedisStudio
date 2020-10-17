using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace RedisStudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDistributedCache DistributedCache;
        private readonly RedisCacheHelper RedisCache;

        public WeatherForecastController(IDistributedCache distributedCache, RedisCacheHelper redisCache)
        {
            DistributedCache = distributedCache;
            RedisCache = redisCache;
        }

        [Route("write")]
        [HttpGet]
        public string Write()
        {
            var cacheKey = "TheTime";
            var existingTime = DateTime.UtcNow.ToString();
            DistributedCache.SetString(cacheKey, existingTime, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
            });

            return "Added to cache : " + existingTime;
        }


        [Route("read")]
        [HttpGet]
        public string Read()
        {
            var cacheKey = "TheTime";
            var existingTime = DistributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return "Fetched from cache : " + existingTime;
            }
            else
            {
                return "null";
            }
        }


        [Route("list")]
        [HttpGet]
        public List<string> GetList()
        {
            var cacheKey = "list";

            var lista = RedisCache.Get<List<string>>(cacheKey);
            if (lista != null)
            {
                return lista;
            }
            else
            {
                RedisCache.Set(cacheKey, new List<string>{"pippo", "pluto", "paperino"}, 60);
                return RedisCache.Get<List<string>>(cacheKey);
            }
        }
    }
}
