using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study.Models;

namespace Iwentys.Domain.Study
{
    public class SubjectAssignment
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime? DeadlineTimeUtc { get; set; }

        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
        public virtual ICollection<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }

        public static SubjectAssignment Create(SubjectTeacher teacher, Subject subject, Assignment assignment)
        {
            return new SubjectAssignment
            {
                Assignment = assignment,
                SubjectId = subject.Id,
                AuthorId = teacher.User.Id
            };
        }

        public SubjectAssignmentSubmit CreateSubmit(AuthorizedUser user, SubjectAssignmentSubmitCreateArguments arguments)
        {
            var canCreateSubmit = Subject.GroupSubjects.Any(gs => gs.StudyGroup.Students.Any(s => s.Id == user.Id));
            if (!canCreateSubmit)
                throw InnerLogicException.SubjectAssignmentException.StudentIsNotAssignedToSubject(user.Id, Id);

            return new SubjectAssignmentSubmit
            {
                StudentId = user.Id,
                SubjectAssignmentId = Id,
                SubmitTimeUtc = DateTime.UtcNow,
                StudentDescription = arguments.StudentDescription
            };
        }
    }
}