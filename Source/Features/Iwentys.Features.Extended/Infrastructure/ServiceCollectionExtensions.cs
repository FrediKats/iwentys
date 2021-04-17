using Iwentys.Features.Companies.Services;
using Iwentys.Features.Newsfeeds.Services;
using Iwentys.Features.PeerReview.Services;
using Iwentys.Features.Raids.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Companies.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysCompanyFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<CompanyService>();

            return services;
        }

        public static IServiceCollection AddIwentysNewsfeedFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<NewsfeedService>();

            return services;
        }

        public static IServiceCollection AddIwentysPeerReviewFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<ProjectReviewService>();

            return services;
        }

        public static IServiceCollection AddIwentysRaidFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<RaidService>();

            return services;
        }
    }
}