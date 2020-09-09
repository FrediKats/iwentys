using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGithubUserDataRepository : IGenericRepository<GithubUserData, int>
    {
        GithubUserData GetUserDataByUsername(string username);
    }
}
