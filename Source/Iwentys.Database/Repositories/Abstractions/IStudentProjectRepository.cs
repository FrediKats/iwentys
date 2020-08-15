using Iwentys.Models.Types.Github;
﻿using System.Collections.Generic;
using Iwentys.Models.Entities;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentProjectRepository : IGenericRepository<StudentProject, long>
    {
        StudentProject GetOrCreate(GithubRepository project, Student creator);
        void CreateMany(IEnumerable<StudentProject> studentsProjects);
        bool Contains(StudentProject project);
        IEnumerable<StudentProject> GetProjectsByUserName(string username);
        StudentProject GetCertainProject(string username, string projectName);
    }
}