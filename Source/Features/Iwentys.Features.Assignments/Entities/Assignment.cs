using System;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class Assignment
    {
        //TODO: replace with init
        public int Id { get; set; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime? Deadline { get; init; }
        public bool IsCompleted { get; private set; }

        public int CreatorId { get; init; }
        public virtual Student Creator { get; init; }

        public int? SubjectId { get; init; }
        public virtual Subject Subject { get; init; }

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