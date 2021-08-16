using Iwentys.Modules.AccountManagement.StudentProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Modules.AccountManagement
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAccountManagementModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(StudentController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(StudentController).Assembly));

            return services;
        }
    }
}
