using System.Threading.Tasks;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IStudentRepository : IGenericRepository<StudentEntity, int>
    {
        Task<StudentEntity> CreateAsync(StudentEntity entity);
    }
}