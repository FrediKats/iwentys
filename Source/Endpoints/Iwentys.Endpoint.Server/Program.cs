using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Iwentys.Endpoint.Server.Source.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;

namespace Iwentys.Endpoint.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        //TODO: return this updates
                        //.ConfigureServices(s => s.AddHostedService<MarkUpdateBackgroundService>())
                        //.ConfigureServices(s => s.AddHostedService<GithubUpdateBackgroundService>())
                        ;
                });
    }
}
