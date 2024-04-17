using CM.RedisCache;
using Microsoft.AspNetCore.Mvc;
using RedisStudio.DbContext;
using RedisStudio.Utility;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisStudio.Controllers;

[ApiController]
[Route("[controller]")]
public class Multiplexer : ControllerBase
{
    private readonly IConnectionMultiplexer multiplexer;
    private readonly MyContext _context;

    public Multiplexer(IConnectionMultiplexer _multiplexer, MyContext context)
    {
        this.multiplexer = _multiplexer;
        _context = context;
    }

    [Route("feed")]
    [HttpGet]
    public IActionResult Feed()
    {
        List<Travel> travels = DbUtility.Feed(10000);

        _context.Travel.AddRange(travels);
        _context.SaveChanges();

        return Ok("Feed OK");
    }

    [Route("db")]
    [HttpGet]
    public async Task<IActionResult> Db()
    {
        //var query = _context.Travel.Where(i => i.Enabled == true && i.City.StartsWith("Port"));
        var nations = new String[]
        {
            "Italy", "Georgia", "Japan"
        };

        var query = _context.Travel.Where(i => i.Enabled == false && nations.Contains(i.Nation));
        
        //var key = query.GetCacheKey();
        var result = await query.GetFromCacheAsync(24);
        return Ok(result);
    }

    [Route("hash")]
    [HttpGet]
    public async Task<IActionResult> Hash()
    {
        RedisCache.HashSet("user.e600418", 
            HashSetBuilder
                .New()
                .Add("code", 1234567)
                .Add("name", "Cristiano")
                .Add("address", "Via Valeriana 14")
                .Add("city", "Sondrio")
                .Build());
        return Ok("OK");
    }

    [Route("hash-object")]
    [HttpGet]
    public async Task<IActionResult> HashObject()
    {
        var travel = DbUtility.Feed(1).First();

        //travel.Latitudine = null;
        //travel.Longitudine = null;

        RedisCache.HashSet("user.e600418", travel);
        return await Task.FromResult(Ok("OK"));

        //var anonymous = new
        //{
        //    name = "Pasquale",
        //    id = 1001,
        //    data = DateTime.Now
        //};

        //RedisCache.HashSet("user.e600418", anonymous);
        //return await Task.FromResult(Ok("OK"));

        // Delete hashset
        //RedisCache.HashSetDelete("user.e600418");
        //return await Task.FromResult(Ok("OK"));
    }

    [Route("gethashall")]
    [HttpGet]
    public async Task<IActionResult> GetHashAll()
    {
        var list = RedisCache.HashSetGetAll("user.e600418");
        return await Task.FromResult(Ok(list));
    }

    [Route("write")]
    [HttpGet]
    public async Task<IActionResult> Write()
    {
        var db = multiplexer.GetDatabase();

        await db.StringSetAsync("key3", "Valore 3.1");
        await db.StringSetAsync("key4", "Valore 4.1");
        await db.StringSetAsync("key5", "Valore 5.1");
        await db.StringSetAsync("key6", "Valore 6.1");

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

    [Route("start")]
    [HttpGet]
    public async Task<IActionResult> Start()
    {
        return await Task.FromResult(Ok("Web app started successfully"));
    }
}