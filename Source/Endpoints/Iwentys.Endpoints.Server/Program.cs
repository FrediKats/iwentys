using Iwentys.Endpoints.OldShared.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iwentys.Endpoints.OldServer
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
                        .ConfigureServices(s => s.AddHostedService<MarkUpdateBackgroundService>())
                        .ConfigureServices(s => s.AddHostedService<GithubUpdateBackgroundService>());
                });
    }
}
