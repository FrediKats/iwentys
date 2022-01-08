using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.PeerReview
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPeerReviewModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(PeerReviewController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(PeerReviewController).Assembly));

            return services;
        }
    }
}
