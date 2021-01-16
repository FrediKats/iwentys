using Iwentys.Features.Guilds.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Guilds.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysGuildFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildRecruitmentService>();
            services.AddScoped<GuildService>();
            services.AddScoped<GuildTestTaskService>();

            return services;
        }
    }
}