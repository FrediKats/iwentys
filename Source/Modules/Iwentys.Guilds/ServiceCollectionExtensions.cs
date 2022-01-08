using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Guilds
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGuildModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GuildController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(GuildController).Assembly));


            return services;
        }
    }
}
