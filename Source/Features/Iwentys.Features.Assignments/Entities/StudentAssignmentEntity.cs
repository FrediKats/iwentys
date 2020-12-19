using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignmentEntity
    {
        public int AssignmentId { get; set; }
        public virtual AssignmentEntity Assignment { get; set; }

        public int StudentId { get; set; }
        public virtual StudentEntity Student { get; set; }

        public static StudentAssignmentEntity Create(StudentEntity creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var assignmentEntity = AssignmentEntity.Create(creator, assignmentCreateRequestDto);
            var studentAssignmentEntity = new StudentAssignmentEntity
            {
                StudentId = creator.Id,
                Assignment = assignmentEntity
            };

            return studentAssignmentEntity;
        }
    }
}