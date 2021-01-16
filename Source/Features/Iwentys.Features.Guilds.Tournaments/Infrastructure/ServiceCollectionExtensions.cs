using Iwentys.Features.Guilds.Tournaments.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds.Tournaments.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGuildTournamentFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<TournamentService>();

            return services;
        }
    }
}