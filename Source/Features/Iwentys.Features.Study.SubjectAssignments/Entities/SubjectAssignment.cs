using System;
using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Features.Study.SubjectAssignments.Entities
{
    public class SubjectAssignment
    {
        public int Id { get; set; }

        public int GroupSubjectId { get; set; }
        public virtual GroupSubject GroupSubject { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        //TODO: add deadline etc?
        //TODO: add author

        public virtual ICollection<SubjectAssignmentSubmit> SubjectAssignmentSubmits { get; set; }

        public static SubjectAssignment Create(AuthorizedUser user, GroupSubject groupSubject, SubjectAssignmentCreateArguments arguments)
        {
            //TODO: allow admins to create assignments?
            if (groupSubject.LectorTeacherId != user.Id && groupSubject.PracticeTeacherId != user.Id)
                throw new InnerLogicException("User is not group teacher");

            return new SubjectAssignment
            {
                GroupSubjectId = groupSubject.SubjectId,
                Title = arguments.Title,
                Description = arguments.Description,
                Link = arguments.Link
            };
        }

        public SubjectAssignmentSubmit CreateSubmit(AuthorizedUser user, SubjectAssignmentSubmitCreateArguments arguments)
        {
            //TODO: ensure user is from this group
            return new SubjectAssignmentSubmit
            {
                StudentId = user.Id,
                SubjectAssignmentId = Id,
                SubmitTimeUtc = DateTime.UtcNow
            };
        }
    }
}