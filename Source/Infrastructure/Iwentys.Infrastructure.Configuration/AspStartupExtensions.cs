using System;
using Iwentys.Infrastructure.Configuration.Options;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure.Configuration
{
    public static class AspStartupExtensions
    {
        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            TokenApplicationOptions token = TokenApplicationOptions.Load(configuration);
            
            return services
                .AddSingleton(token)
                .AddSingleton(new GithubApiAccessorOptions {Token = token.GithubToken})
                .AddSingleton(new ApplicationOptions());
        }

        public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped<IUnitOfWork, UnitOfWork<TContext>>();
            return services;
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}