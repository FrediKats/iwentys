using System;
using System.Collections.Generic;
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

        public virtual ICollection<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }
        public virtual ICollection<SubjectAssignmentSubmit> Submits { get; set; }

        public SubjectAssignment()
        {
            GroupSubjectAssignments = new List<GroupSubjectAssignment>();
            Submits = new List<SubjectAssignmentSubmit>();
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

        public GroupSubjectAssignment AddAssignmentForGroup(IwentysUser user, GroupSubject group)
        {
            SubjectMentor mentor = user.EnsureIsMentor(Subject);
            //TODO: add correct exception
            if (SubjectId != group.SubjectId)
                throw new Exception();

            var groupSubjectAssignment = new GroupSubjectAssignment
            {
                Group = group.StudyGroup,
                SubjectAssignment = this
            };
            GroupSubjectAssignments.Add(groupSubjectAssignment);
            return groupSubjectAssignment;
        }
    }
}