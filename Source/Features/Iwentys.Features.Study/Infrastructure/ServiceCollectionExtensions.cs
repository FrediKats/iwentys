using Iwentys.Features.Study.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Study.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysStudyFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudentService>();
            services.AddScoped<StudyService>();
            services.AddScoped<SubjectActivityService>();
            services.AddScoped<SubjectService>();

            return services;
        }

        public static IServiceCollection AddIwentysAssignmentFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<AssignmentService>();

            return services;
        }
    }
}