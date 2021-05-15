using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Assignments.Models;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Models;

namespace Iwentys.Domain.SubjectAssignments
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
        public virtual ICollection<StudentAssignment> StudentAssignments { get; set; }

        public SubjectAssignment()
        {
            StudentAssignments = new List<StudentAssignment>();
        }

        public static SubjectAssignment Create(IwentysUser user, Subject subject, AssignmentCreateArguments arguments)
        {
            SubjectTeacher teacher = user.EnsureIsTeacher(subject);
            var assignment = Assignment.Create(user, arguments);
            var subjectAssignment = new SubjectAssignment
            {
                Assignment = assignment,
                SubjectId = subject.Id,
                Subject = subject,
                AuthorId = teacher.User.Id
            };

            List<Student> students = subject.GroupSubjects
                .Select(gs => gs.StudyGroup)
                .SelectMany(g => g.Students)
                .ToList();

            foreach (Student student in students)
            {
                subjectAssignment.StudentAssignments.Add(new StudentAssignment
                {
                    Student = student,
                    Assignment = assignment,
                    LastUpdateTimeUtc = DateTime.UtcNow
                });
            }

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