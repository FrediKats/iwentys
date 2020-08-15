using System;
using Iwentys.Core.Services.Abstractions;

namespace Iwentys.Core.Daemons
{
    class GithubUpdateDaemon : DaemonWorker
    {
        private readonly IGithubUserDataService _githubUserDataService;
        public GithubUpdateDaemon(TimeSpan checkInterval, IGithubUserDataService githubUserDataService) : base(checkInterval)
        {
            _githubUserDataService = githubUserDataService;
        }

        protected override void Execute()
        {
            foreach (var githubUserData in _githubUserDataService.GetAll())
            {
                _githubUserDataService.Update(githubUserData.Id);
            }
        }
    }
}
