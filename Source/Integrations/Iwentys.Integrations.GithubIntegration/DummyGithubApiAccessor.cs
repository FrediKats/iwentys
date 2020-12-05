using System;
using System.Collections.Generic;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Octokit;

namespace Iwentys.Integrations.GithubIntegration
{
    public class DummyGithubApiAccessor : IGithubApiAccessor
    {
        public GithubRepositoryInfoDto GetRepository(string username, string repositoryName)
        {
            //TODO: omg, rework this plz
            return new GithubRepositoryInfoDto(17, username, repositoryName, "No desc", null, 0);
        }

        public IReadOnlyList<GithubRepositoryInfoDto> GetUserRepositories(string username)
        {
            return new List<GithubRepositoryInfoDto>();
        }

        public GithubUserInfoDto GetGithubUser(string githubUsername)
        {
            return new GithubUserInfoDto(17, githubUsername, null, "No bio", null);
        }

        public ContributionFullInfo GetUserActivity(string githubUsername)
        {
            return new ContributionFullInfo { RawActivity = new ActivityInfo() { Contributions = new List<ContributionsInfo>(), Years = new List<YearActivityInfo>() } };
        }

        public int GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return default;
        }

        public Organization FindOrganizationInfo(string organizationName)
        {
            return default;
        }
    }
}