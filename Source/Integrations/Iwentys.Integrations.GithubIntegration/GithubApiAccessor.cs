using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Models;
using Octokit;

namespace Iwentys.Integrations.GithubIntegration
{
    public class GithubApiAccessor : IGithubApiAccessor
    {
        private const string GithubContributionsApiUrl = "https://github-contributions.now.sh/api/v1/";

        private readonly GitHubClient _client;

        public GithubApiAccessor(GithubApiAccessorOptions options)
        {
            _client = new GitHubClient(new ProductHeaderValue("Iwentys"))
            {
                Credentials = new Credentials(options.Token)
            };
        }

        public async Task<GithubRepositoryInfoDto> GetRepository(string username, string repositoryName)
        {
            //TODO: remove exception and return null?
            var repository = await _client
                .Repository
                .Get(username, repositoryName) ?? throw EntityNotFoundException.Create(nameof(GithubRepositoryInfoDto), repositoryName);

            return new GithubRepositoryInfoDto(repository.Id, repository.Owner.Login, repository.Name, repository.Description, repository.Url, repository.StargazersCount);
        }

        public async Task<List<GithubRepositoryInfoDto>> GetUserRepositories(string username)
        {
            IReadOnlyList<Repository> repositories = await _client
                .Repository
                .GetAllForUser(username);

            return repositories
                .Select(r => new GithubRepositoryInfoDto(r.Id, r.Owner.Login, r.Name, r.Description, r.Url, r.StargazersCount))
                .ToList();
        }

        public async Task<GithubUserInfoDto> GetGithubUser(string githubUsername)
        {
            var user = await _client
                .User
                .Get(githubUsername) ?? throw EntityNotFoundException.Create(nameof(GithubUserInfoDto), githubUsername);

            return new GithubUserInfoDto(user.Id, user.Name, user.AvatarUrl, user.Bio, user.Company);
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