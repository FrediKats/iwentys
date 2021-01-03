using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; init; }
        public string Description { get; init; }
        public DateTime CreationTimeUtc { get; init; }
        public DateTime? Deadline { get; init; }

        public int CreatorId { get; init; }
        public virtual Student Creator { get; init; }

        public int? SubjectId { get; init; }
        public virtual Subject Subject { get; init; }

        public virtual ICollection<StudentAssignment> StudentAssignments { get; init; }

        public static Assignment Create(Student creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            return new Assignment
            {
                Title = assignmentCreateRequestDto.Title,
                Description = assignmentCreateRequestDto.Description,
                CreationTimeUtc = DateTime.UtcNow,
                Deadline = assignmentCreateRequestDto.Deadline,
                CreatorId = creator.Id,
                SubjectId = assignmentCreateRequestDto.SubjectId
            };
        }

        public StudentAssignment MarkCompleted(Student student)
        {
            StudentAssignment studentAssignment = StudentAssignments.First(sa => sa.StudentId == student.Id);

            studentAssignment.MarkCompleted();
            
            return studentAssignment;
        }

        public StudentAssignment MarkUncompleted(Student student)
        {
            StudentAssignment studentAssignment = StudentAssignments.First(sa => sa.StudentId == student.Id);

            studentAssignment.MarkUncompleted();

            return studentAssignment;
        }
    }
}