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

        Task<ContributionFullInfo> GetUserActivity(string githubUsername);

        Task<int> GetUserActivity(string githubUsername, DateTime from, DateTime to);
        //FYI: this must be implemented after https://github.com/octokit/octokit.net/pull/2239
        //Organization FindOrganizationInfo(string organizationName);
    }
}