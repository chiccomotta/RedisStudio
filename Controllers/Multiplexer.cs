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

        public Multiplexer(IConnectionMultiplexer _multiplexer)
        {
            this.multiplexer = _multiplexer;
        }

        [Route("write")]
        [HttpGet]
        public async Task<IActionResult> Write()
        {
            var db = multiplexer.GetDatabase();
            await db.StringSetAsync("key2", "Valore numero 2....");
            
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
