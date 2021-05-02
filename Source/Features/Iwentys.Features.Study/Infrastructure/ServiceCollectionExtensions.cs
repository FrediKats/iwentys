using Iwentys.Features.Study.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Study.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysStudyFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudyService>();

            return services;
        }
    }
}