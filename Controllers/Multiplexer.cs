using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace RedisStudio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Multiplexer : ControllerBase
    {
        private readonly IConnectionMultiplexer multiplexer;
        private readonly IDatabase redisDatabase;

        public Multiplexer(IConnectionMultiplexer _multiplexer, IDatabase _redisDatabase)
        {
            this.multiplexer = _multiplexer;
            this.redisDatabase = _redisDatabase;
        }

        [Route("write")]
        [HttpGet]
        public async Task<IActionResult> Write()
        {
            //var db = multiplexer.GetDatabase();
            //await db.StringSetAsync("key2", "Valore numero 2....");
            await redisDatabase.StringSetAsync("key3", "Valore 3");

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
    }
}
