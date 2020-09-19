using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Core;
using Iwentys.Core.Daemons;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Api.BackgroundServices
{
    public class GithubUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ILogger _logger;

        public GithubUpdateBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp)
        {
            _sp = sp;
            _logger = loggerFactory.CreateLogger("GithubUpdateBackgroundService");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GithubUpdateBackgroundService start");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _sp.CreateScope();
                    _logger.LogInformation("Execute GithubUpdateBackgroundService update");

                    var githubUpdateDaemon = new GithubUpdateDaemon(
                        scope.ServiceProvider.GetRequiredService<IGithubUserDataService>(),
                        scope.ServiceProvider.GetRequiredService<IStudentRepository>());

                    githubUpdateDaemon.Execute();
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform GithubUpdateBackgroundService update");
                }

                await Task.Delay(ApplicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}