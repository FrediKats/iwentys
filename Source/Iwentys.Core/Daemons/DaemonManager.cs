using System;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories.Abstractions;

namespace Iwentys.Core.Daemons
{
    public static class DaemonManager
    {
        private static MarkUpdateDaemon _markUpdateDaemon;

        public static void Init(ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository)
        {
            //TODO: move interval to config
            _markUpdateDaemon = new MarkUpdateDaemon(
                TimeSpan.FromHours(1),
                new GoogleTableUpdateService(subjectActivityRepository),
                subjectForGroupRepository);
        }

        public static void TryRun()
        {
            _markUpdateDaemon.Notify();
        }
    }
}