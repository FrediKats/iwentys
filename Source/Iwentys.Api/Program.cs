using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Iwentys.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(l =>
                {
                    var builtConfig = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddCommandLine(args)
                        .Build();

                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.File(builtConfig["LogFilePath"])
                        .CreateLogger();
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .ConfigureServices(s => s.AddHostedService<IwentysBackgroundService>());
    }
}
