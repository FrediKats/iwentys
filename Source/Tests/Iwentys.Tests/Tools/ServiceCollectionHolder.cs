using System;
using Iwentys.Database;
using Iwentys.Infrastructure;
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
                .AddIwentysMediatorHandlers()
                .AddIwentysServices();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}