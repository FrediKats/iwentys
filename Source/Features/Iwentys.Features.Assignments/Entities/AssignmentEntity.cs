using System;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class AssignmentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public int CreatorId { get; set; }
        public virtual StudentEntity Creator { get; set; }

        public int? SubjectId { get; set; }
        public virtual SubjectEntity Subject { get; set; }

        public static AssignmentEntity Create(StudentEntity creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            return new AssignmentEntity
            {
                Title = assignmentCreateRequestDto.Title,
                Description = assignmentCreateRequestDto.Description,
                CreationTime = DateTime.UtcNow,
                Deadline = assignmentCreateRequestDto.Deadline,
                CreatorId = creator.Id,
                SubjectId = assignmentCreateRequestDto.SubjectId
            };
        }

        public void MarkCompleted(StudentEntity student)
        {
            if (student.Id != CreatorId)
                throw InnerLogicException.Assignment.IsNotAssignmentCreator(Id, student.Id);
                    
            if (IsCompleted)
                throw InnerLogicException.Assignment.IsAlreadyCompleted(Id);

            IsCompleted = true;
        }

        public void MarkUncompleted(StudentEntity student)
        {
            if (student.Id != CreatorId)
                throw InnerLogicException.Assignment.IsNotAssignmentCreator(Id, student.Id);

            if (!IsCompleted)
                throw InnerLogicException.Assignment.IsNotCompleted(Id);

            IsCompleted = false;
        }
    }
}