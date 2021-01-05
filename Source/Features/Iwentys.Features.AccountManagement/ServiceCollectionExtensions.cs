using Iwentys.Features.AccountManagement.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Features.AccountManagement
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIwentysAAccountManagementFeatureServices(this IServiceCollection services)
        {
            services.AddScoped<IwentysUserService>();

            return services;
        }
    }
}