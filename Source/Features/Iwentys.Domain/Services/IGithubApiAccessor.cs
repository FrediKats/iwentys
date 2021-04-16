using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;

namespace Iwentys.Domain.Services
{
    public interface IGithubApiAccessor
    {
        Task<GithubRepositoryInfoDto> GetRepository(string username, string repositoryName);
        Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username);
        Task<GithubUserInfoDto> GetGithubUser(string githubUsername);

        Task<ContributionFullInfo> GetUserActivity(string githubUsername);

        Task<int> GetUserActivity(string githubUsername, DateTime from, DateTime to);
        OrganizationInfoDto FindOrganizationInfo(string organizationName);
    }
}