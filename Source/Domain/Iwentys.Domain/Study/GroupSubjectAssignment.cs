namespace Iwentys.Domain.Study
{
    public class GroupSubjectAssignment
    {
        public int SubjectAssignmentId { get; set; }
        public virtual SubjectAssignment SubjectAssignment { get; set; }

        public int GroupId { get; set; }
        public virtual StudyGroup Group { get; set; }
    }
}