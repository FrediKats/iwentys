using Iwentys.Common.Databases;
using Iwentys.Database;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Features.Extended.Infrastructure;
using Iwentys.Features.Gamification.Infrastructure;
using Iwentys.Features.GithubIntegration.Infrastructure;
using Iwentys.Features.Guilds.Infrastructure;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Infrastructure.Options;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            TokenApplicationOptions token = TokenApplicationOptions.Load(configuration);
            
            return services
                .AddSingleton(IsuApplicationOptions.Load(configuration))
                .AddSingleton(token)
                .AddSingleton(JwtApplicationOptions.Load(configuration))
                .AddSingleton(new GithubApiAccessorOptions {Token = token.GithubToken})
                .AddSingleton(new ApplicationOptions());
        }

        public static IServiceCollection AddIwentysServices(this IServiceCollection services)
        {
            //FYI: replace after release
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            //services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddIwentysEconomyFeatureServices();
            services.AddIwentysGamificationFeatureServices();
            services.AddIwentysGithubIntegrationFeatureServices();

            services.AddIwentysGuildFeatureServices();

            services.AddIwentysTributesFeatureServices();
            services.AddIwentysQuestFeatureServices();
            
            services.AddScoped<IStudyDbContext, IwentysDbContext>();

            services.AddIwentysRaidFeatureServices();

            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }
    }
}