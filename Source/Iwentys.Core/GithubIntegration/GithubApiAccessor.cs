using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Iwentys.Core.Tools;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Github;
using Newtonsoft.Json;
using Octokit;

namespace Iwentys.Core.GithubIntegration
{
    public class GithubApiAccessor : IGithubApiAccessor
    {
        private readonly GitHubClient _client;

        public GithubApiAccessor()
        {
            //TODO: Move token to repo secrets
            _client = new GitHubClient(new ProductHeaderValue("Iwentys"))
            {
                Credentials = new Credentials(String.Empty)
            };
        }

        public GithubRepository GetRepository(string username, string repositoryName)
        {
            return _client
                .Repository
                .Get(username, repositoryName)
                .Result
                .Maybe(r => new GithubRepository(r.Id, r.Name, r.Description, r.Url)) ?? throw EntityNotFoundException.Create(nameof(GithubRepository), repositoryName);
        }

        public IReadOnlyList<GithubRepository> GetUserRepositories(string username)
        {
            return _client
                .Repository
                .GetAllForUser(username)
                .Result
                .Select(r => new GithubRepository(r.Id, r.Name, r.Description, r.Url))
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
            using (var http = new HttpClient())
            {
                string info = http.GetStringAsync($"https://github-contributions-api.now.sh/v1/{githubUsername}").Result;
                var result = JsonConvert.DeserializeObject<ActivityInfo>(info);
                List<ContributionsInfo> perMonth = result
                    .Contributions
                    .GroupBy(c => c.Date.Substring(0, 7))
                    .Select(c => new ContributionsInfo(c.Key, c.Sum(_ => _.Count)))
                    .ToList();

                return new ContributionFullInfo()
                {
                    PerMonthActivity = perMonth,
                    RawActivity = result
                };
            }
        }
    }
}