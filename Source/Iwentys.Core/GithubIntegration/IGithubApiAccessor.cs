using System;
using System.Collections.Generic;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Core.GithubIntegration
{
    public interface IGithubApiAccessor
    {
        GithubRepository GetRepository(string username, string repositoryName);
        IReadOnlyList<GithubRepository> GetUserRepositories(string username);

        GithubUser GetGithubUser(string githubUsername);

        ContributionFullInfo GetUserActivity(string githubUsername);
        int GetUserActivity(string githubUsername, DateTime from, DateTime to);
    }
}