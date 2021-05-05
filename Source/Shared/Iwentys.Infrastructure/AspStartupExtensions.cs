using System;
using Iwentys.Common.Databases;
using Iwentys.Database;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Features.Extended.Companies;
using Iwentys.Features.Gamification.Quests;
using Iwentys.Features.Guilds.Guilds;
using Iwentys.Features.Guilds.Infrastructure;
using Iwentys.Features.Study.Infrastructure;
using Iwentys.Features.Study.StudentProfile;
using Iwentys.Infrastructure.Options;
using Iwentys.Integrations.GithubIntegration;
using MediatR;
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

        public static IServiceCollection AddIwentysMediatorHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(CompanyController).Assembly);
            services.AddMediatR(typeof(QuestController).Assembly);
            services.AddMediatR(typeof(GuildController).Assembly);
            services.AddMediatR(typeof(StudentController).Assembly);

            return services;
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}