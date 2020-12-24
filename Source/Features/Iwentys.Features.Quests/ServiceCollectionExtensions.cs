using Iwentys.Features.Quests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Quests
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysQuestFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<QuestService>();

            return services;
        }
    }
}