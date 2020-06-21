using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Core.Tools;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;
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
    }
}