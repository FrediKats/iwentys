using Iwentys.Features.PeerReview.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.PeerReview.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysPeerReviewFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<ProjectReviewService>();

            return services;
        }
    }
}