using Iwentys.Modules.Study.Study;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Modules.Study
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStudyModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(SubjectController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(SubjectController).Assembly));

            return services;
        }
    }
}
