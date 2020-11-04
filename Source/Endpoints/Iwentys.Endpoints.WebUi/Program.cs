using Google.Apis.Logging;
using Iwentys.Endpoints.Shared.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Iwentys.Endpoints.WebUi
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
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog()
                .ConfigureServices(s => s.AddHostedService<MarkUpdateBackgroundService>())
                .ConfigureServices(s => s.AddHostedService<GithubUpdateBackgroundService>());
    }
}
