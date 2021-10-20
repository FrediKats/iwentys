using Iwentys.Modules.SubjectAssignments.MentorScope;
using MediatR;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Modules.SubjectAssignments
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSubjectAssignmentsModule(this IServiceCollection services)
        {
            services.AddMediatR(typeof(MentorSubjectAssignmentController).Assembly);

            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(typeof(MentorSubjectAssignmentController).Assembly));

            return services;
        }
    }
}