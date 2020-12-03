using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration.Entities;

namespace Iwentys.Features.GithubIntegration.Repositories
{
    public interface IGithubUserDataRepository : IGenericRepository<GithubUserEntity, int>
    {
        GithubUserEntity Create(GithubUserEntity entity);
        Task<GithubUserEntity> FindByUsernameAsync(string username);
    }
}