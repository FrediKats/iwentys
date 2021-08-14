using System;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Configuration;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Modules.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Tests.Tools
{
    public class ServiceCollectionHolder
    {
        public ServiceProvider ServiceProvider { get; }

        public ServiceCollectionHolder()
        {
            IServiceCollection services = new ServiceCollection()
                .AddDbContext<IwentysDbContext>(options => options
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .UseLazyLoadingProxies())
                .AddUnitOfWork<IwentysDbContext>()
                .AddIwentysSeeder()
                .AddIwentysMediatorHandlers()
                .AddIwentysServices()
                .AddAutoMapperConfig()
                .AddGuildModule();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}