using Microsoft.Extensions.DependencyInjection;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Iwentys.Endpoint.Client.Tools;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Iwentys.Endpoint.Client
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("Iwentys.Endpoint.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddBlazoredLocalStorage();
            //builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Iwentys.Endpoint.ServerAPI"));

            builder.Services.AddApiAuthorization();

            return builder.Build().RunAsync();
        }
    }
}
