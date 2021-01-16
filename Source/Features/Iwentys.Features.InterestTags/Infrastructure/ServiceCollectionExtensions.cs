using Iwentys.Features.InterestTags.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.InterestTags.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysInterestTagFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<InterestTagService>();

            return services;
        }
    }
}