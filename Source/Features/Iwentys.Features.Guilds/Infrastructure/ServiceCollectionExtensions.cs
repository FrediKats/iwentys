using Iwentys.Features.Guilds.Services;
using Iwentys.Features.Guilds.Tributes.Services;
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

        public static IServiceCollection AddIwentysTributesFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<GuildTributeService>();

            return services;
        }
    }
}