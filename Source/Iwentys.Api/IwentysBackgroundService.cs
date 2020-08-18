using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Core;
using Iwentys.Core.Daemons;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api
{
    public class IwentysBackgroundService : BackgroundService
    {
        private static MarkUpdateDaemon _markUpdateDaemon;
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;

        public IwentysBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            _sp = sp;
            _logger = loggerFactory.CreateLogger("DaemonManager");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IwentysBackgroundService start");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _sp.CreateScope();
                    _logger.LogInformation("Tun worker");

                    var accessor = scope.ServiceProvider.GetRequiredService<DatabaseAccessor>();
                    _markUpdateDaemon = new MarkUpdateDaemon(
                        ApplicationOptions.DaemonUpdateInterval,
                        new GoogleTableUpdateService(_logger, accessor.SubjectActivity, accessor.Student),
                        accessor.SubjectForGroup,
                        _logger);

                    _markUpdateDaemon.Execute();

                    await Task.Delay(ApplicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform dispatch");
                }
                
            }
        }
    }
}