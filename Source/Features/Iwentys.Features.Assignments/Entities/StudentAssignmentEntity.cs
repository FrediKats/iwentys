using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignmentEntity
    {
        public int AssignmentId { get; set; }
        public virtual AssignmentEntity Assignment { get; set; }

        public int StudentId { get; set; }
        public virtual StudentEntity Student { get; set; }
    }
}