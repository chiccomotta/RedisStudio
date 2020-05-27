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

        [HttpGet]
        public string Get()
        {
            var cacheKey = "TheTime";
            var existingTime = DistributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(existingTime))
            {
                return "Fetched from cache : " + existingTime;
            }
            else
            {
                existingTime = DateTime.UtcNow.ToString();
                DistributedCache.SetString(cacheKey, existingTime);
                return "Added to cache : " + existingTime;
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
                RedisCache.Set(cacheKey, new List<string>{"pippo", "pluto", "paperino"});
                return RedisCache.Get<List<string>>(cacheKey);
            }
        }
    }
}
