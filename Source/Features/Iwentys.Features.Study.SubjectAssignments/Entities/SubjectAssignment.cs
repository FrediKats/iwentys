using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.SubjectAssignments.Domain;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Features.Study.SubjectAssignments.Entities
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
                AssignmentId = assignment.Id,
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