using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace RedisStudio
{
    public class RedisSubscriber : BackgroundService
    {
        private readonly IConnectionMultiplexer ConnectionMultiplexer;

        public RedisSubscriber(IConnectionMultiplexer connectionMultiplexer)
        {
            ConnectionMultiplexer = connectionMultiplexer;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var subscriber = ConnectionMultiplexer.GetSubscriber();
            return subscriber.SubscribeAsync("messages", (channel, value) =>
            {
                Debug.WriteLine($"Message content was: {value}");
            });
        }
    }
}
