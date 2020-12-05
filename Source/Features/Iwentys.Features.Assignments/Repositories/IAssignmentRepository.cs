using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Assignments.Repositories
{
    public interface IAssignmentRepository
    {
        Task<StudentAssignmentEntity> CreateAsync(StudentEntity creator, AssignmentCreateRequestDto assignmentCreateRequestDto);
        IQueryable<StudentAssignmentEntity> Read();
        Task<AssignmentEntity> MarkCompletedAsync(int assignmentId);
        Task DeleteAsync(int assignmentId);
    }
}