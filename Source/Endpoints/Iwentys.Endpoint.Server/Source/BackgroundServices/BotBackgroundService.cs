using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Endpoint.Server.Source.Tools;
using Iwentys.Endpoints.ClientBot;
using Iwentys.Endpoints.ClientBot.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Tef.BotFramework.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
{
    public class BotBackgroundService : BackgroundService
    {
        private readonly ILogger _logger;

        public BotBackgroundService(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("BotBackgroundService");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IwentysBackgroundService start");
            throw new NotImplementedException();

#pragma warning disable CS0162 // Unreachable code detected
            try
#pragma warning restore CS0162 // Unreachable code detected
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddIwentysServices();
                Bot bot = BotProvider.Init(new TelegramDebugSettings(ApplicationOptions.TelegramToken), Log.Logger, serviceCollection);
                bot.Start();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Bot run failed");
            }

            return Task.CompletedTask;
        }
    }
}