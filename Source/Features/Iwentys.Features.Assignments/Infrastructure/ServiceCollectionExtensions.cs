using Iwentys.Features.Assignments.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Assignments.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysAssignmentFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<AssignmentService>();

            return services;
        }
    }
}