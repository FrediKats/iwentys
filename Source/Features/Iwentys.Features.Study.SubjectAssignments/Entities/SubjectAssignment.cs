using System;
using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
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

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public DateTime LastUpdateTimeUtc { get; set; }
        public DateTime? DeadlineTimeUtc { get; set; }

        public int AuthorId { get; set; }
        public virtual IwentysUser Author { get; set; }

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
        public virtual ICollection<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }

        public static SubjectAssignment Create(SubjectTeacher teacher, GroupSubject groupSubject, SubjectAssignmentCreateArguments arguments)
        {
            return new SubjectAssignment
            {
                SubjectId = groupSubject.SubjectId,
                Title = arguments.Title,
                Description = arguments.Description,
                Link = arguments.Link,
                AuthorId = teacher.User.Id
            };
        }

        public SubjectAssignmentSubmit CreateSubmit(AuthorizedUser user, SubjectAssignmentSubmitCreateArguments arguments)
        {
            //TODO: ensure user is from this group
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