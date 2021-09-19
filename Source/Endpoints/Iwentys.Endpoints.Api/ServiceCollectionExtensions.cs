using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.AccountManagement;
using Iwentys.Modules.Gamification;
using Iwentys.Modules.Guilds;
using Iwentys.Modules.PeerReview;
using Iwentys.Modules.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Iwentys.Endpoints.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysIdentity(this IServiceCollection services)
        {
            //TODO: load from config
            return services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=identity.db"))
                .ConfigureIdentityFramework();
        }

        public static IServiceCollection EnableExceptional(this IServiceCollection services)
        {
            return services
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddExceptional(settings => { settings.Store.ApplicationName = "Samples.AspNetCore"; });
        }

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

        public static IServiceCollection AddIwentysModules(this IServiceCollection services)
        {
            services
                .AddAccountManagementModule()
                .AddGamificationModule()
                .AddGuildModule()
                .AddPeerReviewModule()
                .AddStudyModule();

            return services;
        }
    }
}