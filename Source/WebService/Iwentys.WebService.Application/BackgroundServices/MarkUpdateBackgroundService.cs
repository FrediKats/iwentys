using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Iwentys.WebService.Application
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

                    var context = scope.ServiceProvider.GetRequiredService<IwentysDbContext>();
                    
                    var googleTableUpdateService = new MarkGoogleTableUpdateService(_logger, _tokenApplicationOptions.GoogleServiceToken, context);

                    foreach (GroupSubject g in context.GroupSubjects.ToList())
                    {
                        try
                        {
                            await googleTableUpdateService.UpdateSubjectActivityForGroup(g);
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