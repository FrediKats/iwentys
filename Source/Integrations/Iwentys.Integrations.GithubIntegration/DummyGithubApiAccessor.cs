using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Octokit;

namespace Iwentys.Integrations.GithubIntegration
{
    public class DummyGithubApiAccessor : IGithubApiAccessor
    {
        public Task<GithubRepositoryInfoDto> GetRepository(string username, string repositoryName)
        {
            var result = new GithubRepositoryInfoDto(17, username, repositoryName, "No desc", null, 0);
            //TODO: It hack. Need to implement this methods for test propose
            return Task.FromResult(result);
        }

        public Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username)
        {
            return Task.FromResult(new List<GithubRepositoryInfoDto>());
        }

        public Task<GithubUserInfoDto> GetGithubUser(string githubUsername)
        {
            var result = new GithubUserInfoDto(17, githubUsername, null, "No bio", null);
            return Task.FromResult(result);
        }

        public Task<ContributionFullInfo> GetUserActivity(string githubUsername)
        {
            var result = new ContributionFullInfo { RawActivity = new ActivityInfo() { Contributions = new List<ContributionsInfo>(), Years = new List<YearActivityInfo>() } };
            return Task.FromResult(result);
        }

        public Task<int> GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return Task.FromResult(0);
        }

        public Organization FindOrganizationInfo(string organizationName)
        {
            return default;
        }
    }
}