using System;
using Iwentys.Infrastructure.Configuration.Options;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Seeding;
using Iwentys.Integrations.GithubIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Infrastructure.Configuration
{
    public static class IwentysApiStartupExtensions
    {
        public static IServiceCollection AddIwentysOptions(this IServiceCollection services, IConfiguration configuration)
        {
            TokenApplicationOptions token = TokenApplicationOptions.Load(configuration);
            
            return services
                .AddSingleton(token)
                .AddSingleton(new GithubApiAccessorOptions {Token = token.GithubToken})
                .AddSingleton(new ApplicationOptions());
        }

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
        {
            return services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        public static IServiceCollection AddIwentysSeeder(this IServiceCollection services)
        {
            return services.AddScoped<IDbContextSeeder, DatabaseContextGenerator>();
        }
    }
}