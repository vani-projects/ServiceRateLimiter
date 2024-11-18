
using Vani.Comminication.Contracts;
using Vani.Comminication.Helper;
using Vani.Comminication.Repositories;
using Vani.Comminication.Service;

namespace Vani.Comminication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var messageRateLimits = new MessageRateLimits();
            configuration.GetSection(IMessageRateLimits.CONFIG_SECTION_TITLE).Bind(messageRateLimits);

            var builder = WebApplication.CreateBuilder(args);

            

            // Add services to the container.
            builder.Services.AddTransient<IMessageRateLimits, MessageRateLimits>();

            builder.Services.AddSingleton(new InMemoryRateLimiter(messageRateLimits.PhoneNumberRateLimit, messageRateLimits.AccountRateLimit));
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
