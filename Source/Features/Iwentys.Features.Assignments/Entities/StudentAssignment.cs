using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignment
    {
        public bool IsCompleted { get; private set; }
        public DateTime LastUpdateTime { get; private set; }

        public int AssignmentId { get; init; }
        public virtual Assignment Assignment { get; init; }

        public int StudentId { get; init; }
        public virtual Student Student { get; init; }

        public static StudentAssignment Create(Student creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var assignmentEntity = Assignment.Create(creator, assignmentCreateRequestDto);
            var studentAssignmentEntity = new StudentAssignment
            {
                StudentId = creator.Id,
                Assignment = assignmentEntity,
                LastUpdateTime = DateTime.UtcNow
            };

            return studentAssignmentEntity;
        }

        public static List<StudentAssignment> CreateForGroup(GroupAdminUser groupAdmin, AssignmentCreateRequestDto assignmentCreateRequestDto, StudyGroup studyGroup)
        {
            var assignmentEntity = Assignment.Create(groupAdmin.Student, assignmentCreateRequestDto);
            
            List<StudentAssignment> studentAssignmentEntities = studyGroup.Students.Select(s => new StudentAssignment
            {
                StudentId = s.Id,
                Assignment = assignmentEntity,
                LastUpdateTime = DateTime.UtcNow
            }).ToList();

            return studentAssignmentEntities;
        }

        public void MarkCompleted()
        {
            if (IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsAlreadyCompleted(AssignmentId);

            IsCompleted = true;
            LastUpdateTime = DateTime.UtcNow;
        }

        public void MarkUncompleted()
        {
            if (!IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsNotCompleted(AssignmentId);
            
            IsCompleted = false;
            LastUpdateTime = DateTime.UtcNow;
        }
    }
}