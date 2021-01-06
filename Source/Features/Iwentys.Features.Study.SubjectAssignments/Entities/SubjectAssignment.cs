using System;
using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Entities;
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
        public IwentysUser Author { get; set; }

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }
        public virtual ICollection<GroupSubjectAssignment> GroupSubjectAssignments { get; set; }

        public static SubjectAssignment Create(IwentysUser user, GroupSubject groupSubject, SubjectAssignmentCreateArguments arguments)
        {
            if (groupSubject.LectorTeacherId != user.Id && groupSubject.PracticeTeacherId != user.Id && !user.IsAdmin)
                throw new InnerLogicException("User is not group teacher");

            return new SubjectAssignment
            {
                SubjectId = groupSubject.SubjectId,
                Title = arguments.Title,
                Description = arguments.Description,
                Link = arguments.Link,
                AuthorId = user.Id
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