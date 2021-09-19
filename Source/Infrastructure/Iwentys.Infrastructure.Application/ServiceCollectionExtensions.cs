using System;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.Schedule;
using Iwentys.Infrastructure.Application.Options;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Seeding;
using Iwentys.Integrations.GithubIntegration;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysServices(this IServiceCollection services)
        {
            //FYI: replace after release
            services.AddScoped<IGithubApiAccessor, DummyGithubApiAccessor>();
            //services.AddScoped<IGithubApiAccessor, GithubApiAccessor>();
            services.AddScoped<AchievementProvider>();

            services.AddScoped<GithubIntegrationService>();



            return services;
        }

        public static IServiceCollection AddIwentysMediatorHandlers(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ScheduleController).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,,>));

            return services;
        }

        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            TokenApplicationOptions token = TokenApplicationOptions.Load(configuration);

            return services
                .AddSingleton(token)
                .AddSingleton(new GithubApiAccessorOptions { Token = token.GithubToken })
                .AddSingleton(new ApplicationOptions());
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IServiceCollection AddIwentysSeeder(this IServiceCollection services)
        {
            return services.AddScoped<IDbContextSeeder, DatabaseContextGenerator>();
        }
    }
}