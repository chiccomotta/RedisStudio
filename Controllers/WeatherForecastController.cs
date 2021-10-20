using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisStudio.Models;

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
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(120)
            });

            return "Added to cache : " + existingTime;
        }

        [Route("write2")]
        [HttpGet]
        public string Write2()
        {
            var list = new List<FooClass>();

            for (int i = 0; i < 100; i++)
            {
                var t = new FooClass()
                {
                    BPDossierConfigId = 1,
                    CAT = "44030",
                    CATDescription = "Very long description",
                    DefaultEvent = "99",
                    CATDetail = "Test",
                    OA = Guid.NewGuid().ToString(),
                    InsertDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    MP = Guid.NewGuid().ToString(),
                    MPDescription = Guid.NewGuid().ToString()
                };

                list.Add(t);
            }
            
            var cacheKey = "TheList";
            DistributedCache.SetString(cacheKey, JsonSerializer.Serialize(list) , new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            });

            return "Added to cache : ";
        }

        [Route("read2")]
        [HttpGet]
        public ActionResult Read2()
        {
            var t1 = new Stopwatch();
            t1.Start();

            var cacheKey = "TheList";
            var list = DistributedCache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(list))
            {
                //var res = JsonSerializer.Deserialize<IEnumerable<FooClass>>(list);
                t1.Stop();

                var tm = t1.Elapsed.ToString(@"m\:ss\.fff");
                return Ok(list + Environment.NewLine + $"{tm}");
            }
            else
            {
                return NotFound();
            }
        }

        [Route("read")]
        [HttpGet]
        public string Read()
        {
            /*
             * In redis-cli.exe per leggere il dato di tipo hash usare: hgetall <instanceName>TheTime
             * Per vedere tutte le chiavi: keys *
             * informazioni sul server: info server 
             * cambiare db: select <index>
             *
             */

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

        [Route("time")]
        [HttpGet]
        public IActionResult GetTime()
        {
            var dt =  DateTime.Parse("2020-10-26T07:20:00Z");
            var timeUtc = dt.ToUniversalTime();

            var lista = new List<string>();

            try
            {
                //var localZone = TimeZone.CurrentTimeZone;
                //var res = localZone.IsDaylightSavingTime(dt);

                var zone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, zone);

                //foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
                //{

                //    //TimeZoneInfo zone = TZConvert.GetTimeZoneInfo("W. Europe Standard Time");
                //    DateTime cstTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, z);

                //var t = zone.IsDaylightSavingTime(cstTime) ? zone.DaylightName : zone.StandardName;

                lista.Add($"{zone.Id} {cstTime:hh:mm} - {zone}");
                //}

                //var tizoneCurrent = TimeZone.CurrentTimeZone;
                //TimeZoneInfo zone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
    }
}
