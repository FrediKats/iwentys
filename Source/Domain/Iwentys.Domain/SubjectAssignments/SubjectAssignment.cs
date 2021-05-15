using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Domain.SubjectAssignments
{
    public class SubjectAssignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime? DeadlineTimeUtc { get; set; }

        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        public SubjectAssignment()
        {
        }

        public static SubjectAssignment Create(IwentysUser user, Subject subject, SubjectAssignmentCreateArguments arguments)
        {
            SubjectMentor mentor = user.EnsureIsMentor(subject);
            var subjectAssignment = new SubjectAssignment
            {
                Title = arguments.Title,
                Description = arguments.Description,
                SubjectId = subject.Id,
                Subject = subject,

                Author = user,
                AuthorId = mentor.Mentor.Id,
                CreationTimeUtc = DateTime.UtcNow,
                LastUpdateTimeUtc = DateTime.UtcNow,
                DeadlineTimeUtc = arguments.DeadlineUtc
            };

            return subjectAssignment;
        }

        public SubjectAssignmentSubmit CreateSubmit(IwentysUser user, SubjectAssignmentSubmitCreateArguments arguments)
        {
            bool canCreateSubmit = Subject.GroupSubjects.Any(gs => gs.StudyGroup.Students.Any(s => s.Id == user.Id));
            if (!canCreateSubmit)
                throw InnerLogicException.SubjectAssignmentException.StudentIsNotAssignedToSubject(user.Id, Id);

            return new SubjectAssignmentSubmit
            {
                StudentId = user.Id,
                SubjectAssignmentId = Id,
                SubjectAssignment = this,
                SubmitTimeUtc = DateTime.UtcNow,
                StudentDescription = arguments.StudentDescription
            };
        }
    }
}