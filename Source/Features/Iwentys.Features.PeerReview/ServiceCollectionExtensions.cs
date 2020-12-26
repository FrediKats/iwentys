using Iwentys.Features.Tributes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Tributes
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysTributesFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildTributeService>();

            return services;
        }
    }
}