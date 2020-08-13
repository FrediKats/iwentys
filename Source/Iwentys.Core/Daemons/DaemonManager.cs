using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.Daemons
{
    public static class DaemonManager
    {
        private static MarkUpdateDaemon _markUpdateDaemon;

        public static void Init(ILogger logger, ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository)
        {
            _markUpdateDaemon = new MarkUpdateDaemon(
                ApplicationOptions.DaemonUpdateInterval,
                new GoogleTableUpdateService(logger, subjectActivityRepository),
                subjectForGroupRepository);
        }

        public static void TryRun()
        {
            _markUpdateDaemon.Notify();
        }
    }
}