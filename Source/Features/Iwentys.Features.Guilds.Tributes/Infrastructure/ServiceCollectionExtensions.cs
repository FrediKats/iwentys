using Iwentys.Features.Guilds.Tributes.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds.Tributes.Infrastructure
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