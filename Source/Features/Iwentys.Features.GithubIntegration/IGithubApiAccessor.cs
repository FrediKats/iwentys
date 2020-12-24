using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.GithubIntegration
{
    public interface IGithubApiAccessor
    {
        Task<GithubRepositoryInfoDto> GetRepository(string username, string repositoryName);
        Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username);
        Task<GithubUserInfoDto> GetGithubUser(string githubUsername);

        ContributionFullInfo GetUserActivity(string githubUsername);
        int GetUserActivity(string githubUsername, DateTime from, DateTime to);
        //TODO: fix
        //Organization FindOrganizationInfo(string organizationName);
    }
}