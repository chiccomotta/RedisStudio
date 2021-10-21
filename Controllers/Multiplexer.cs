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

            var tt = multiplexer.Configuration;
            await db.StringSetAsync("key1", "Value-1");
            
            return Ok("OK");
        }

        [Route("read")]
        [HttpGet]
        public async Task<IActionResult> Read()
        {
            var db = multiplexer.GetDatabase();
            var tt = multiplexer.Configuration;
            var value = await db.StringGetAsync("key1");
            
            return Ok(value.ToString());
        }
    }
}
