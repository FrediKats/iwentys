using Iwentys.Features.Students.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.Students
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysStudentsFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<StudentService>();

            return services;
        }
    }
}