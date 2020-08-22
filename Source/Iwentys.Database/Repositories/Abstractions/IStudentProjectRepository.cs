using System.Collections.Generic;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Github;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentProjectRepository : IGenericRepository<StudentProject, long>
    {
        StudentProject GetOrCreate(GithubRepository project, Student creator);
        void CreateMany(IEnumerable<StudentProject> studentsProjects);
        bool Contains(StudentProject project);
        IEnumerable<StudentProject> FindProjectsByUserName(string username);
        StudentProject FindCertainProject(string username, string projectName);
    }
}