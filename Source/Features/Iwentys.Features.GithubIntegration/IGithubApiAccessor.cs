using System;
using System.Collections.Generic;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.GithubIntegration
{
    public interface IGithubApiAccessor
    {
        GithubRepository GetRepository(string username, string repositoryName);
        IReadOnlyList<GithubRepository> GetUserRepositories(string username);

        GithubUser GetGithubUser(string githubUsername);

        ContributionFullInfo GetUserActivity(string githubUsername);
        int GetUserActivity(string githubUsername, DateTime from, DateTime to);
        //TODO: fix
        //Organization FindOrganizationInfo(string organizationName);
    }
}