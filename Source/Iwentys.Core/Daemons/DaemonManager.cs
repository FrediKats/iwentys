using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Context;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.Daemons
{
    public static class DaemonManager
    {
        private static MarkUpdateDaemon _markUpdateDaemon;

        public static void Init(ILogger logger, DatabaseAccessor databaseAccessor)
        {
            _markUpdateDaemon = new MarkUpdateDaemon(
                ApplicationOptions.DaemonUpdateInterval,
                new GoogleTableUpdateService(logger, databaseAccessor.SubjectActivity, databaseAccessor.Student),
                databaseAccessor.SubjectForGroup);
        }

        public static void TryRun()
        {
            _markUpdateDaemon.Notify();
        }
    }
}