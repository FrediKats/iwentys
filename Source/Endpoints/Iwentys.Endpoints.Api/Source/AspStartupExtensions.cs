using Iwentys.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Iwentys.Endpoints.Api.Source
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection AddIwentysLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("Logs/iwentys-{Date}.log")
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            return services;
        }

        //FYI: Need to rework CORS after release
        public static IServiceCollection AddIwentysCorsHack(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            return services;
        }

        public static IServiceCollection AddIwentysDatabase(this IServiceCollection services)
        {
            //FYI: need to replace with normal db after release
            //services.AddDbContext<IwentysDbContext>(o => o.UseSqlite("Data Source=Iwentys.db"));
            services
                .AddDbContext<IwentysDbContext>(o => o
                    .UseLazyLoadingProxies()
                    .UseInMemoryDatabase("Data Source=Iwentys.db"));
            return services;
        }
    }
}