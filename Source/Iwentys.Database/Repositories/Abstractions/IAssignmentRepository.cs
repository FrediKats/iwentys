using System.Linq;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IAssignmentRepository
    {
        StudentAssignmentEntity Create(StudentEntity creator, AssignmentCreateDto assignmentCreateDto);
        IQueryable<StudentAssignmentEntity> Read();
    }
}