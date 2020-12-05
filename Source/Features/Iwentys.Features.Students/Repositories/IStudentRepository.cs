using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Students.Repositories
{
    public interface IStudentRepository : IGenericRepository<StudentEntity, int>
    {
        Task<StudentEntity> CreateAsync(StudentEntity entity);
    }
}