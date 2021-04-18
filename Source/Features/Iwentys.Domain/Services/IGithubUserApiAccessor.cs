using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Models;

namespace Iwentys.Domain.Services
{
    public interface IGithubUserApiAccessor
    {
        Task<GithubUser> CreateOrUpdate(int studentId);
        Task<List<GithubUser>> GetAllGithubUser();
        Task<ContributionFullInfo> FindUserContributionOrEmpty(IwentysUser student, bool useCache = true);
        Task<GithubUser> GetGithubUser(string username, bool useCache = true);
        Task<GithubUser> Get(int studentId, bool useCache = true);
        Task<GithubUser> FindGithubUser(int studentId, bool useCache = true);
        Task<GithubUser> ForceRescanUser(IwentysUser student, GithubUser oldGithubUser);
    }
}