using System.Threading;
using System.Threading.Tasks;
using Iwentys.Api.Tools;
using Iwentys.ClientBot;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Tef.BotFramework.Core;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Iwentys.Api
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
            
            Bot bot = BotProvider.Init("https://localhost:3578", new TelegramDebugSettings(), Log.Logger);
            bot.Start();

            return Task.CompletedTask;
        }
    }
}