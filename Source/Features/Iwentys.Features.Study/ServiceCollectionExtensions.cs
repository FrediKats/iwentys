using Iwentys.Features.Study.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Study
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysStudyFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudyGroupService>();
            services.AddScoped<SubjectActivityService>();
            services.AddScoped<SubjectService>();

            return services;
        }
    }
}