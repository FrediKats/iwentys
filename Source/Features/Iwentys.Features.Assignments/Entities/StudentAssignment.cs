using System;
using System.Collections.Generic;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignment
    {
        public bool IsCompleted { get; private set; }
        public DateTime LastUpdateTimeUtc { get; private set; }

        public int AssignmentId { get; init; }
        public virtual Assignment Assignment { get; init; }

        public int StudentId { get; init; }
        public virtual Student Student { get; init; }

        public static StudentAssignment Create(IwentysUser author, AssignmentCreateArguments createArguments)
        {
            var assignmentEntity = Assignment.Create(author, createArguments);
            var studentAssignmentEntity = new StudentAssignment
            {
                StudentId = author.Id,
                Assignment = assignmentEntity,
                LastUpdateTimeUtc = DateTime.UtcNow
            };

            return studentAssignmentEntity;
        }

        public static List<StudentAssignment> CreateForGroup(GroupAdminUser groupAdmin, AssignmentCreateArguments createArguments)
        {
            var assignment = Assignment.Create(groupAdmin.Student, createArguments);
            List<StudyGroupMember> groupMembers = groupAdmin.Student.GroupMember.Group.Students;

            List<StudentAssignment> studentAssignments = groupMembers.SelectToList(s => new StudentAssignment
            {
                StudentId = s.StudentId,
                Assignment = assignment,
                LastUpdateTimeUtc = DateTime.UtcNow
            });

            return studentAssignments;
        }

        public void MarkCompleted()
        {
            if (IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsAlreadyCompleted(AssignmentId);

            IsCompleted = true;
            LastUpdateTimeUtc = DateTime.UtcNow;
        }

        public void MarkUncompleted()
        {
            if (!IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsNotCompleted(AssignmentId);
            
            IsCompleted = false;
            LastUpdateTimeUtc = DateTime.UtcNow;
        }
    }
}