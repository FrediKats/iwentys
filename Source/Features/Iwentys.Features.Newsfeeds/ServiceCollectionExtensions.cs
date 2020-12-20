using Iwentys.Features.Newsfeeds.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Newsfeeds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysNewsfeedFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<NewsfeedService>();

            return services;
        }
    }
}