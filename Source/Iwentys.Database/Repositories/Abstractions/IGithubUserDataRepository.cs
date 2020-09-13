using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGithubUserDataRepository : IGenericRepository<GithubUserData, int>
    {
        //TODO: check
        GithubUserData Create(GithubUserData githubUserData);

        GithubUserData FindByUsername(string username);
    }
}
