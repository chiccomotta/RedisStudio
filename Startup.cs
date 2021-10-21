using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace RedisStudio
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Questa configurazione funziona
            //services.AddStackExchangeRedisCache(options =>
            //{
            //    //options.Configuration = "eaitdsrv009-ncox:6379, defaultDatabase=2";
            //    options.Configuration = "127.0.0.1:6379, defaultDatabase=2";            
                
            //    // Prefisso per evitare conflitti nel nome delle chiavi con altre applicazioni
            //    options.InstanceName = "app-";
                
            //    //  #####     REDIS-CLI     ######

            //    //   IN REDIS-CLI.EXE PER LEGGERE IL DATO DI TIPO HASH USARE: HGETALL <INSTANCENAME>THETIME
            //    //   PER VEDERE TUTTE LE CHIAVI: KEYS *
            //    //   INFORMAZIONI SUL SERVER: INFO SERVER 
            //    //   CAMBIARE DB: SELECT <INDEX>

            //});

            //services.AddSingleton<RedisCacheHelper>();
            
            // Altra configurazione che utilizza direttamente la classe Multiplexer
            services.AddSingleton<IConnectionMultiplexer>(opt =>
                ConnectionMultiplexer.Connect(Configuration.GetValue<string>("RedisConnection")));

            // Aggiungo un BackgroundService che resta in ascolto dei messaggi di Redis
            services.AddHostedService<RedisSubscriber>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
