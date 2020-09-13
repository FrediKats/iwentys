using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Core;
using Iwentys.Core.Daemons;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api
{
    public class IwentysBackgroundService : BackgroundService
    {
        private MarkUpdateDaemon _markUpdateDaemon;
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;

        public IwentysBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            _sp = sp;
            _logger = loggerFactory.CreateLogger("IwentysBackgroundService");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("IwentysBackgroundService start");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _sp.CreateScope();
                    _logger.LogInformation("Execute IwentysBackgroundService update");

                    ProcessMarkUpdateSafe(scope);
                    ProcessGithubUpdateSafe(scope);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform IwentysBackgroundService update");
                }

                await Task.Delay(ApplicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }

        private void ProcessGithubUpdateSafe(IServiceScope scope)
        {
            try
            {
                var githubUpdateDaemon = new GithubUpdateDaemon(
                    scope.ServiceProvider.GetRequiredService<IGithubUserDataService>(),
                    scope.ServiceProvider.GetRequiredService<IStudentRepository>());

                githubUpdateDaemon.Execute();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Github data update failed.");
            }
        }

        private void ProcessMarkUpdateSafe(IServiceScope scope)
        {
            try
            {
                var accessor = scope.ServiceProvider.GetRequiredService<DatabaseAccessor>();

                _markUpdateDaemon = new MarkUpdateDaemon(
                    new GoogleTableUpdateService(_logger, accessor.SubjectActivity, accessor.Student),
                    accessor.GroupSubject,
                    _logger);

                _markUpdateDaemon.Execute();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Mark data update failed.");
            }
        }
    }
}