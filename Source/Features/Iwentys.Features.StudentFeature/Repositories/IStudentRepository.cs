using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities;

namespace Iwentys.Features.StudentFeature.Repositories
{
    public interface IStudentRepository : IGenericRepository<StudentEntity, int>
    {
        Task<StudentEntity> CreateAsync(StudentEntity entity);
    }
}