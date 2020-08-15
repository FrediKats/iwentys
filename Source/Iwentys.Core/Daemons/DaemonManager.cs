using Iwentys.Core.GithubIntegration;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Core.Services.Implementations;
using Iwentys.Database.Repositories.Abstractions;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.Daemons
{
    public static class DaemonManager
    {
        private static MarkUpdateDaemon _markUpdateDaemon;
        private static GithubUpdateDaemon _githubUpdateDaemon;

        public static void Init(ILogger logger, ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository,
            IGithubUserDataRepository githubUserDataRepository, IGithubApiAccessor githubApiAccessor, IStudentRepository studentRepository, IStudentProjectRepository studentProjectRepository)
        {
            _markUpdateDaemon = new MarkUpdateDaemon(
                ApplicationOptions.DaemonUpdateInterval,
                new GoogleTableUpdateService(logger, subjectActivityRepository),
                subjectForGroupRepository);

            _githubUpdateDaemon = new GithubUpdateDaemon(
                ApplicationOptions.DaemonUpdateInterval,
                new GithubUserDataService(githubUserDataRepository, githubApiAccessor, studentRepository, studentProjectRepository));
        }

        public static void TryRun()
        {
            _markUpdateDaemon.Notify();
        }
    }
}