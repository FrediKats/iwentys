using Iwentys.Domain.Gamification;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Modules.Guilds.Guilds;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Modules.Guilds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGuildModule(this IServiceCollection services)
        {
            services.AddScoped<GuildMemberService>();
            services.AddScoped<GuildService>();
            services.AddMediatR(typeof(GuildController).Assembly);


            return services;
        }
    }
}
