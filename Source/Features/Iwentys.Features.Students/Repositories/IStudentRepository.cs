using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Students.Repositories
{
    public interface IStudentRepository : IGenericRepository<StudentEntity, int>
    {
        Task<StudentEntity> CreateAsync(StudentEntity entity);
    }

    public static class StudentRepositoryExtensions
    {
        public static async Task UpdateGithub(this IStudentRepository repository, int studentId, string username)
        {
            StudentEntity user = await repository.GetAsync(studentId);
            user.GithubUsername = username;
            await repository.UpdateAsync(user);
        }
    }
}