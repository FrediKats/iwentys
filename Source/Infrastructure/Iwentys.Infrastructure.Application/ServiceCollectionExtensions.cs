using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.Companies;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Integrations.GithubIntegration;
using MediatR;
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
            services.AddMediatR(typeof(CompanyController).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipeline<,,>));

            return services;
        }
    }
}