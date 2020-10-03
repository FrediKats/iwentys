using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGithubUserDataService
    {
        GithubUserEntity CreateOrUpdate(int studentId);
        GithubUserEntity FindByUsername(string username);
        IEnumerable<GithubRepository> GetGithubRepositories(string username);
        GithubRepository GetCertainRepository(string username, string projectName);
        IEnumerable<GithubUserEntity> GetAll();
    }
}