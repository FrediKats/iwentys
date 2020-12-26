using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Endpoint.Server.Source.Options;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
{
    public class MarkUpdateBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _sp;
        private readonly TokenApplicationOptions _tokenApplicationOptions;
        private readonly ApplicationOptions _applicationOptions;
        private readonly ILogger _logger;

        public MarkUpdateBackgroundService(ILoggerFactory loggerFactory, IServiceProvider sp, TokenApplicationOptions tokenApplicationOptions, ApplicationOptions applicationOptions)
        {
            _sp = sp;
            _tokenApplicationOptions = tokenApplicationOptions;
            _applicationOptions = applicationOptions;
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

                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    IGenericRepository<GroupSubjectEntity> groupSubjectRepository = unitOfWork.GetRepository<GroupSubjectEntity>();
                    var subjectActivityRepository = scope.ServiceProvider.GetRequiredService<ISubjectActivityRepository>();
                    
                    var googleTableUpdateService = new MarkGoogleTableUpdateService(subjectActivityRepository, _logger, _tokenApplicationOptions.GoogleServiceToken, unitOfWork);

                    foreach (GroupSubjectEntity g in groupSubjectRepository.Get().ToList())
                    {
                        try
                        {
                            googleTableUpdateService.UpdateSubjectActivityForGroup(g);
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, "Fail to perform MarkUpdateBackgroundService update");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Fail to perform MarkUpdateBackgroundService update");
                }

                await Task.Delay(_applicationOptions.DaemonUpdateInterval, stoppingToken).ConfigureAwait(false);
            }
        }
    }
}