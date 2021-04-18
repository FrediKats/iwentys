using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;

namespace Iwentys.Domain
{
    public class StudentAssignment
    {
        public bool IsCompleted { get; private set; }
        public DateTime LastUpdateTimeUtc { get; set; }

        public int AssignmentId { get; init; }
        public virtual Assignment Assignment { get; init; }

        public int StudentId { get; init; }
        public virtual Student Student { get; init; }

        public static List<StudentAssignment> Create(Student author, AssignmentCreateArguments createArguments)
        {
            if (createArguments.ForStudyGroup)
            {
                return CreateForGroup(author.EnsureIsGroupAdmin(), createArguments);
            }
            else
            {
                return new List<StudentAssignment> {CreateSingle(author, createArguments)};
            }
        }

        public static StudentAssignment CreateSingle(IwentysUser author, AssignmentCreateArguments createArguments)
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
            List<Student> groupMembers = groupAdmin.Student.Group.Students;

            List<StudentAssignment> studentAssignments = groupMembers.SelectToList(s => new StudentAssignment
            {
                StudentId = s.Id,
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