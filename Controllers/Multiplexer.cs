using System;
using Microsoft.AspNetCore.Mvc;
using RedisStudio.DbContext;
using StackExchange.Redis;
using System.Linq;
using System.Threading.Tasks;
using CM.RedisCache;

namespace RedisStudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Multiplexer : ControllerBase
    {
        private readonly IConnectionMultiplexer multiplexer;
        private readonly IDatabase redisDatabase;
        private readonly MyContext _context;

        public Multiplexer(IConnectionMultiplexer _multiplexer, IDatabase _redisDatabase, MyContext context)
        {
            this.multiplexer = _multiplexer;
            this.redisDatabase = _redisDatabase;
            _context = context;
        }

        [Route("db")]
        [HttpGet]
        public IActionResult Db()
        {
            var query =  _context.Travel.Where(i => i.Enabled == true);
            //var key = query.GetCacheKey();

            var result = query.FromCache(new TimeSpan(8,0,0));

            return Ok(result);
        }

        [Route("write")]
        [HttpGet]
        public async Task<IActionResult> Write()
        {
            await redisDatabase.StringSetAsync("key3", "Valore 3.1");
            await redisDatabase.StringSetAsync("key4", "Valore 4.1");
            await redisDatabase.StringSetAsync("key5", "Valore 5.1");
            await redisDatabase.StringSetAsync("key6", "Valore 6.1");

            return Ok("OK");
        }

        [Route("read/{key}")]
        [HttpGet]
        public async Task<IActionResult> Read(string key)
        {
            var db = multiplexer.GetDatabase();
            var value = await db.StringGetAsync(key);

            return Ok(value.ToString());
        }

        [Route("pub")]
        [HttpGet]
        public async Task<IActionResult> Publish()
        {
            var subscriber = multiplexer.GetSubscriber();

            for (int i = 0; i < 20; i++)
            {
                await subscriber.PublishAsync("messages", "TEST-----" +  i);
            }

            return Ok("OK");
        }

        /// <summary>
        /// Esempio di LIST Data Structure in Redis
        /// </summary>
        /// <returns></returns>
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var db = multiplexer.GetDatabase();
            var key = "myList";

            //var server = multiplexer.GetServer("127.0.0.1", 6379);
            // await server.FlushDatabaseAsync(3);  SET AllowAdmin = TRUE

            db.ListLeftPush(key, new RedisValue[]
            {
                "valore 1",
                "valore 2",
                "valore 3",
                "valore 4",
                "valore 5",
                "valore 6"
            });

            var list = db.ListRange(key, 0, 2);
            var t = list.ToStringArray();

            return Ok(t);
        }
    }
}
