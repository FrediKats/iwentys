using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.ViewModels;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Assignments.Repositories
{
    public interface IAssignmentRepository
    {
        Task<StudentAssignmentEntity> CreateAsync(StudentEntity creator, AssignmentCreateRequest assignmentCreateRequest);
        IQueryable<StudentAssignmentEntity> Read();
    }
}