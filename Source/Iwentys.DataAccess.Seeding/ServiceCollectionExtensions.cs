using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.DataAccess.Seeding;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIwentysSeeder(this IServiceCollection services)
    {
        return services.AddScoped<IDbContextSeeder, DatabaseContextGenerator>();
    }
}