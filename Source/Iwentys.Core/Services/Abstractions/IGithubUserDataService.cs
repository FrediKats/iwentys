using System.Collections.Generic;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGithubUserDataService
    {
        GithubUserData Create(int studentId, string username);
        GithubUserData Update(int id);
        GithubUserData GetUserDataByUsername(string username);
        IEnumerable<GithubRepository> GetGithubRepositories(string username);
        GithubRepository GetCertainRepository(string username, string projectName);
        IEnumerable<GithubUserData> GetAll();
    }
}
