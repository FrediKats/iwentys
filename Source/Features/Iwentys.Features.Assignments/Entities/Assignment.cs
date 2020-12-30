using System;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }

        public int CreatorId { get; set; }
        public virtual Student Creator { get; set; }

        public int? SubjectId { get; set; }
        public virtual Subject Subject { get; set; }

        public static Assignment Create(Student creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            return new Assignment
            {
                Title = assignmentCreateRequestDto.Title,
                Description = assignmentCreateRequestDto.Description,
                CreationTime = DateTime.UtcNow,
                Deadline = assignmentCreateRequestDto.Deadline,
                CreatorId = creator.Id,
                SubjectId = assignmentCreateRequestDto.SubjectId
            };
        }

        public void MarkCompleted(Student student)
        {
            if (student.Id != CreatorId)
                throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(Id, student.Id);
                    
            if (IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsAlreadyCompleted(Id);

            IsCompleted = true;
        }

        public void MarkUncompleted(Student student)
        {
            if (student.Id != CreatorId)
                throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(Id, student.Id);

            if (!IsCompleted)
                throw InnerLogicException.AssignmentExceptions.IsNotCompleted(Id);

            IsCompleted = false;
        }
    }
}