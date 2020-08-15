using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentProjectRepository : IGenericRepository<StudentProject, int>
    {
        void CreateMany(IEnumerable<StudentProject> studentsProjects);
        bool Contains(StudentProject project);
        IEnumerable<StudentProject> GetProjectsByUserName(string username);
        StudentProject GetCertainProject(string username, string projectName);
    }
}