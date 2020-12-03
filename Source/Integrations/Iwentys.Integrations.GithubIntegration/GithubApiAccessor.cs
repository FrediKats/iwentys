using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Integrations.GithubIntegration.Models;
using Newtonsoft.Json;
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

        public GithubRepository GetRepository(string username, string repositoryName)
        {
            return _client
                .Repository
                .Get(username, repositoryName)
                .Result
                .Maybe(r => new GithubRepository(r.Id, r.Owner.Login, r.Name, r.Description, r.Url, r.StargazersCount)) ?? throw EntityNotFoundException.Create(nameof(GithubRepository), repositoryName);
        }

        public IReadOnlyList<GithubRepository> GetUserRepositories(string username)
        {
            return _client
                .Repository
                .GetAllForUser(username)
                .Result
                .Select(r => new GithubRepository(r.Id, r.Owner.Login, r.Name, r.Description, r.Url, r.StargazersCount))
                .ToList();
        }

        public GithubUser GetGithubUser(string githubUsername)
        {
            return _client
                .User
                .Get(githubUsername)
                .Result
                .Maybe(u => new GithubUser(u.Name, u.AvatarUrl, u.Bio, u.Company)) ?? throw EntityNotFoundException.Create(nameof(GithubUser), githubUsername);
        }

        public ContributionFullInfo GetUserActivity(string githubUsername)
        {
            using var http = new HttpClient();

            string info = http.GetStringAsync(GithubContributionsApiUrl + githubUsername).Result;
            var result = JsonConvert.DeserializeObject<ActivityInfo>(info);

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