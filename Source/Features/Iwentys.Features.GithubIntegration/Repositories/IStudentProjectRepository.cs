using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Features.GithubIntegration.Repositories
{
    public interface IStudentProjectRepository : IGenericRepository<GithubProjectEntity, long>
    {
        GithubProjectEntity Create(GithubProjectEntity entity);
        Task<GithubProjectEntity> GetOrCreateAsync(GithubRepository project, StudentEntity creator);
        void CreateMany(IEnumerable<GithubProjectEntity> studentsProjects);
        bool Contains(GithubProjectEntity projectEntity);
        IEnumerable<GithubProjectEntity> FindProjectsByUserName(string username);
        GithubProjectEntity FindCertainProject(string username, string projectName);
    }
}