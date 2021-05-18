using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazor.Extensions.Logging;
using Blazored.LocalStorage;
using Iwentys.Endpoints.WebClient.IdentityAuthorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VxFormGenerator.Settings.Bootstrap;

namespace Iwentys.Endpoints.WebClient
{
    public class Program
    {
        public static Task Main(string[] args)
        {

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services
                .AddHttpClient("Iwentys.Endpoint.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            builder.Services
                .AddApiAuthorization()
                .AddAccountClaimsPrincipalFactory<CustomUserFactory>();


            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddVxFormGenerator();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Iwentys.Endpoint.ServerAPI"));

            builder.Services.AddLogging(b => b
                .AddBrowserConsole()
                .SetMinimumLevel(LogLevel.Information)
            );

            return builder.Build().RunAsync();
        }
    }
}
