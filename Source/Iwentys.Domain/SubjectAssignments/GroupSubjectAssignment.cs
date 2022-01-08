using System.Collections.Generic;
using Iwentys.Common;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Domain.SubjectAssignments
{
    public class GroupSubjectAssignment
    {
        public int SubjectAssignmentId { get; set; }
        public virtual SubjectAssignment SubjectAssignment { get; set; }

        public int GroupId { get; set; }
        public virtual StudyGroup Group { get; set; }

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        public GroupSubjectAssignment()
        {
            SubjectAssignmentSubmits = new List<SubjectAssignmentSubmit>();
        }

        public SubjectAssignmentSubmit CreateSubmit(Student student, SubjectAssignmentSubmitCreateArguments arguments)
        {
            if (student.GroupId != Group.Id)
                throw InnerLogicException.SubjectAssignmentException.StudentIsNotAssignedToSubject(student.Id, SubjectAssignment.Id);

            var subjectAssignmentSubmit = new SubjectAssignmentSubmit(student, SubjectAssignment, arguments);

            SubjectAssignmentSubmits.Add(subjectAssignmentSubmit);
            return subjectAssignmentSubmit;
        }
    }
}