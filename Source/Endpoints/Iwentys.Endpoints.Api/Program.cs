using Iwentys.Endpoints.Api.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Iwentys.Endpoints.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureServices(s => s.AddHostedService<BotBackgroundService>())
                .ConfigureServices(s => s.AddHostedService<MarkUpdateBackgroundService>())
                .ConfigureServices(s => s.AddHostedService<GithubUpdateBackgroundService>());
    }
}
