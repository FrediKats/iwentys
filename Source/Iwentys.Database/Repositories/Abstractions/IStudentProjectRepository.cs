using Iwentys.Models.Entities;
using Iwentys.Models.Types.Github;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IStudentProjectRepository : IGenericRepository<StudentProject, long>
    {
        StudentProject GetOrCreate(GithubRepository project, Student creator);
    }
}