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

namespace Iwentys.Api.BackgroundServices
{
    public class MarkUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;

        public MarkUpdateBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            _sp = sp;
            _logger = loggerFactory.CreateLogger("MarkUpdateBackgroundService");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("MarkUpdateBackgroundService start");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _sp.CreateScope();
                    _logger.LogInformation("Execute MarkUpdateBackgroundService update");

                    var accessor = scope.ServiceProvider.GetRequiredService<DatabaseAccessor>();

                    var markUpdateDaemon = new MarkUpdateDaemon(
                        new GoogleTableUpdateService(_logger, accessor.SubjectActivity, accessor.Student),
                        accessor.GroupSubject,
                        _logger);

                    markUpdateDaemon.Execute();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform MarkUpdateBackgroundService update");
                }

                await Task.Delay(ApplicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}