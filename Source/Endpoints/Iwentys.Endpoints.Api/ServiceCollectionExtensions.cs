using Iwentys.Infrastructure.Application.Authorization;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.AccountManagement;
using Iwentys.Modules.Gamification;
using Iwentys.Modules.Guilds;
using Iwentys.Modules.PeerReview;
using Iwentys.Modules.Study;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Endpoints.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO: load from config
            return services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source=identity.db"))
                .ConfigureIdentityFramework(configuration);
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

        public static IServiceCollection EnableExceptional(this IServiceCollection services)
        {
            return services
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddExceptional(settings => { settings.Store.ApplicationName = "Samples.AspNetCore"; });
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