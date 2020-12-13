using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
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

                    var studentRepository = scope.ServiceProvider.GetRequiredService<IStudentRepository>();
                    var githubUserDataService = scope.ServiceProvider.GetRequiredService<GithubIntegrationService>();
                    foreach (StudentEntity student in studentRepository.Read().Where(s => s.GithubUsername is not null))
                    {
                        await githubUserDataService.CreateOrUpdate(student.Id);
                    }
                }
                catch (InvalidOperationException operationException)
                {
                    _logger.LogError(operationException, "Probably some services was not load.");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    continue;

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