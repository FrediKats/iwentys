using System.Collections.Generic;
using Iwentys.Models;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentProjectRepository : IGenericRepository<GithubProjectEntity, long>
    {
        //TODO: check
        GithubProjectEntity Create(GithubProjectEntity githubProject);

        GithubProjectEntity GetOrCreate(GithubRepository project, StudentEntity creator);
        void CreateMany(IEnumerable<GithubProjectEntity> studentsProjects);
        bool Contains(GithubProjectEntity projectEntity);
        IEnumerable<GithubProjectEntity> FindProjectsByUserName(string username);
        GithubProjectEntity FindCertainProject(string username, string projectName);
    }
}