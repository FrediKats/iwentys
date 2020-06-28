using System;
using System.Collections.Generic;
using Iwentys.Models.Types.Github;

namespace Iwentys.Core.GithubIntegration
{
    public class DummyGithubApiAccessor : IGithubApiAccessor
    {
        public GithubRepository GetRepository(string username, string repositoryName) => default;
        public IReadOnlyList<GithubRepository> GetUserRepositories(string username) => new List<GithubRepository>();
        public GithubUser GetGithubUser(string githubUsername) => default;
        public ContributionFullInfo GetUserActivity(string githubUsername) => default;
        public int GetUserActivity(string githubUsername, DateTime @from, DateTime to) => default;
    }
}