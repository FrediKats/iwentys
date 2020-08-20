using System.Collections.Generic;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Core.GithubIntegration
{
    public class DummyGithubUserDataService : IGithubUserDataService
    {
        public GithubUserData Create(int studentId, string username)
        {
            return new GithubUserData();
        }

        public GithubUserData Update(int id)
        {
            return new GithubUserData();
        }

        public GithubUserData GetUserDataByUsername(string username)
        {
            return new GithubUserData
                {ContributionFullInfo = new ContributionFullInfo {PerMonthActivity = new List<ContributionsInfo>()}};
        }

        public IEnumerable<GithubRepository> GetGithubRepositories(string username)
        {
            return new List<GithubRepository>();
        }

        public GithubRepository GetCertainRepository(string username, string projectName)
        {
            return new GithubRepository(-1, $"{username}/{projectName}", "No desc", null, 0);
        }

        public IEnumerable<GithubUserData> GetAll()
        {
            return new List<GithubUserData>();
        }
    }
}
