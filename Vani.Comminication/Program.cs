
using StackExchange.Redis;
using Vani.Comminication.Config;
using Vani.Comminication.Service;

namespace Vani.Comminication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var messageRateLimits = new MessageRateLimitsConfig();
            configuration.GetSection(MessageRateLimitsConfig.CONFIG_SECTION_TITLE).Bind(messageRateLimits);

            var rateLimitsResourceExpiryInSeconds = Convert.ToInt32(configuration["RateLimitsResourceExpiryInSeconds"]);

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // START: In-Memory dictionary as a store for keeping track of the call count
            builder.Services.AddSingleton<IRateLimiterService, InMemoryRateLimiterService>();
            // END: In-Memory dictionary as a store for keeping track of the call count

            // START: Redis Cache as store for keeping track of the call count
            // builder.Services.AddSingleton<IRateLimiterService,RedisRateLimiterService>();
            // END:  Redis Cache as store for keeping track of the call count

            builder.Services.AddHostedService<CleanupService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
           

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
