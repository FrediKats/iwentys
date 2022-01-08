using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.GithubIntegration.Models;
using Kysect.GithubActivityAnalyzer.ApiAccessor;
using Kysect.GithubActivityAnalyzer.ApiAccessor.ApiResponses;
using Octokit;

namespace Iwentys.Integrations.GithubIntegration
{
    public class GithubApiAccessor : IGithubApiAccessor
    {
        private const string GithubContributionsApiUrl = "https://github-contributions.now.sh/api/v1/";

        private readonly GitHubClient _client;
        private readonly GithubActivityProvider _activityProvider;

        public GithubApiAccessor(GithubApiAccessorOptions options)
        {
            _activityProvider = new GithubActivityProvider();
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

            return new GithubRepositoryInfoDto(repository.Id, repository.Owner.Login, repository.Name, repository.Description, repository.HtmlUrl, repository.StargazersCount);
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

        public async Task<ContributionFullInfo> GetUserActivity(string githubUsername)
        {
            ActivityInfo activity = await _activityProvider.GetActivityInfo(githubUsername);

            return new ContributionFullInfo
            {
                RawActivity = MapToDomainModel(activity),
            };
        }

        public async Task<int> GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            var activity = await GetUserActivity(githubUsername);
            return activity.GetActivityForPeriod(from, to);
        }

        public OrganizationInfoDto FindOrganizationInfo(string organizationName)
        {
            Organization organization = _client.Organization.Get(organizationName).Result;
            return new OrganizationInfoDto()
            {
                Name = organization.Name,
                Description = organization.Description
            };
        }

        private CodingActivityInfo MapToDomainModel(ActivityInfo activity)
        {
            return new CodingActivityInfo()
            {
                Years = new List<Domain.GithubIntegration.Models.YearActivityInfo>(),
                Contributions = activity.Contributions.Select(x => new Domain.GithubIntegration.Models.ContributionsInfo(x.Date, x.Count)).ToList()
            };
        }
    }
}