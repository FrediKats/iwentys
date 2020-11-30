using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.ViewModels;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;

namespace Iwentys.Features.Assignments.Repositories
{
    public interface IAssignmentRepository
    {
        Task<StudentAssignmentEntity> CreateAsync(StudentEntity creator, AssignmentCreateRequest assignmentCreateRequest);
        IQueryable<StudentAssignmentEntity> Read();
    }
}