using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Voting.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysVotingFeatureServices(this IServiceCollection services)
        {
            return services;
        }
    }
}