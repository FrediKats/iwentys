using Iwentys.Features.Guilds.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGuildFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildRecruitmentService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildTestTaskService>();
            services.AddScoped<GuildTributeService>();
            services.AddScoped<TournamentService>();

            return services;
        }
    }
}