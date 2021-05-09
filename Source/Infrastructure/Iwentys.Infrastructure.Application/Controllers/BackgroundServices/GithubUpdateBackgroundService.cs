using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Configuration.Options;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Infrastructure.Application.Controllers.BackgroundServices
{
    public class GithubUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly ApplicationOptions _applicationOptions;
        private readonly ILogger _logger;

        public GithubUpdateBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp, ApplicationOptions applicationOptions)
        {
            _sp = sp;
            _applicationOptions = applicationOptions;
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

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var studentRepository = unitOfWork.GetRepository<Student>();
                    var githubUserDataService = scope.ServiceProvider.GetRequiredService<GithubIntegrationService>();
                    foreach (Student student in studentRepository.Get().Where(s => s.GithubUsername != null))
                    {
                        await githubUserDataService.User.CreateOrUpdate(student.Id);
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

                await Task.Delay(_applicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}