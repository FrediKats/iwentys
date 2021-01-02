using Iwentys.Features.Study.SubjectAssignments.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Study.SubjectAssignments
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysSubjectAssignmentFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<SubjectAssignmentService>();

            return services;
        }
    }
}