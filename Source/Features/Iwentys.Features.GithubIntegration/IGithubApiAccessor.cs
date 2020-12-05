using System;
using System.Collections.Generic;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.GithubIntegration
{
    public interface IGithubApiAccessor
    {
        GithubRepositoryInfoDto GetRepository(string username, string repositoryName);
        IReadOnlyList<GithubRepositoryInfoDto> GetUserRepositories(string username);

        GithubUserInfoDto GetGithubUser(string githubUsername);

        ContributionFullInfo GetUserActivity(string githubUsername);
        int GetUserActivity(string githubUsername, DateTime from, DateTime to);
        //TODO: fix
        //Organization FindOrganizationInfo(string organizationName);
    }
}