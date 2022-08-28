using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.WebClient.Sdk;

public static class ServiceCollectionExtensions
{
    public static void RegisterSwaggerClients(this IServiceCollection service)
    {
        List<Type> clients = typeof(ServiceCollectionExtensions).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.Name.EndsWith("Client")).ToList();

        foreach (var client in clients)
            service.AddScoped(client);
    }
}