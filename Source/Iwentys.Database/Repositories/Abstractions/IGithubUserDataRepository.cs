using Iwentys.Models.Entities.Github;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGithubUserDataRepository : IGenericRepository<GithubUserEntity, int>
    {
        //TODO: check
        GithubUserEntity Create(GithubUserEntity githubUserEntity);

        GithubUserEntity FindByUsername(string username);
    }
}