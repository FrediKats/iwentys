using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Newtonsoft.Json;
using Octokit;

namespace Iwentys.Core.GithubIntegration
{
    public class GithubApiAccessor : IGithubApiAccessor
    {
        private const string GithubContributionsApiUrl = "https://github-contributions.now.sh/api/v1/";

        private readonly GitHubClient _client;

        public GithubApiAccessor()
        {
            _client = new GitHubClient(new ProductHeaderValue("Iwentys"))
            {
                Credentials = new Credentials(ApplicationOptions.GithubToken)
            };
        }

        public GithubRepository GetRepository(string username, string repositoryName)
        {
            return _client
                .Repository
                .Get(username, repositoryName)
                .Result
                .Maybe(r => new GithubRepository(r.Id, r.Name, r.Description, r.Url, r.StargazersCount)) ?? throw EntityNotFoundException.Create(nameof(GithubRepository), repositoryName);
        }

        public IReadOnlyList<GithubRepository> GetUserRepositories(string username)
        {
            return _client
                .Repository
                .GetAllForUser(username)
                .Result
                .Select(r => new GithubRepository(r.Id, r.Name, r.Description, r.Url, r.StargazersCount))
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
            List<ContributionsInfo> perMonth = result
                .Contributions
                .GroupBy(c => c.Date.Substring(0, 7))
                .Select(c => new ContributionsInfo(c.Key, c.Sum(_ => _.Count)))
                .ToList();

            return new ContributionFullInfo
            {
                PerMonthActivity = perMonth,
                RawActivity = result
            };
        }

        public int GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return GetUserActivity(githubUsername)
                .RawActivity
                .Contributions
                .Select(c => (Date: DateTime.Parse(c.Date), c.Count))
                .Where(c => c.Date >= from && c.Date <= to)
                .Sum(c => c.Count);
        }

        public Organization FindOrganizationInfo(string organizationName)
        {
            Organization organization = _client.Organization.Get(organizationName).Result;
            return organization;
        }
    }
}