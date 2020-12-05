using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Octokit;

namespace Iwentys.Integrations.GithubIntegration
{
    public class GithubApiAccessor : IGithubApiAccessor
    {
        //TODO: rework
        public static string Token;
        private const string GithubContributionsApiUrl = "https://github-contributions.now.sh/api/v1/";

        private readonly GitHubClient _client;

        public GithubApiAccessor()
        {
            _client = new GitHubClient(new ProductHeaderValue("Iwentys"))
            {
                Credentials = new Credentials(Token)
            };
        }

        public GithubRepositoryInfoDto GetRepository(string username, string repositoryName)
        {
            return _client
                .Repository
                .Get(username, repositoryName)
                .Result
                .Maybe(r => new GithubRepositoryInfoDto(r.Id, r.Owner.Login, r.Name, r.Description, r.Url, r.StargazersCount)) ?? throw EntityNotFoundException.Create(nameof(GithubRepositoryInfoDto), repositoryName);
        }

        public IReadOnlyList<GithubRepositoryInfoDto> GetUserRepositories(string username)
        {
            return _client
                .Repository
                .GetAllForUser(username)
                .Result
                .Select(r => new GithubRepositoryInfoDto(r.Id, r.Owner.Login, r.Name, r.Description, r.Url, r.StargazersCount))
                .ToList();
        }

        public GithubUserInfoDto GetGithubUser(string githubUsername)
        {
            return _client
                .User
                .Get(githubUsername)
                .Result
                .Maybe(u => new GithubUserInfoDto(u.Id, u.Name, u.AvatarUrl, u.Bio, u.Company)) ?? throw EntityNotFoundException.Create(nameof(GithubUserInfoDto), githubUsername);
        }

        public ContributionFullInfo GetUserActivity(string githubUsername)
        {
            using var http = new HttpClient();

            string info = http.GetStringAsync(GithubContributionsApiUrl + githubUsername).Result;
            var result = JsonSerializer.Deserialize<ActivityInfo>(info);

            return new ContributionFullInfo
            {
                RawActivity = result
            };
        }

        public int GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return GetUserActivity(githubUsername).GetActivityForPeriod(from, to);
        }

        public Organization FindOrganizationInfo(string organizationName)
        {
            Organization organization = _client.Organization.Get(organizationName).Result;
            return organization;
        }
    }
}