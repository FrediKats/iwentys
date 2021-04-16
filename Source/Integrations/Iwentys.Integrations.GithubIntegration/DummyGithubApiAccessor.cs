using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using Iwentys.Features.GithubIntegration;

namespace Iwentys.Integrations.GithubIntegration
{
    public class DummyGithubApiAccessor : IGithubApiAccessor
    {
        public Task<GithubRepositoryInfoDto> GetRepository(string username, string repositoryName)
        {
            return Task.FromResult<GithubRepositoryInfoDto>(default);
        }

        public Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username)
        {
            return Task.FromResult(new List<GithubRepositoryInfoDto>());
        }

        public Task<GithubUserInfoDto> GetGithubUser(string githubUsername)
        {
            return Task.FromResult<GithubUserInfoDto>(default);
        }

        public Task<ContributionFullInfo> GetUserActivity(string githubUsername)
        {
            return Task.FromResult<ContributionFullInfo>(default);
        }

        public Task<int> GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return Task.FromResult(0);
        }

        public OrganizationInfoDto FindOrganizationInfo(string organizationName)
        {
            return default;
        }
    }
}