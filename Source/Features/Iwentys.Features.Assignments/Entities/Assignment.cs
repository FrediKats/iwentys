using System;
using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Title { get; init; }
        public string Description { get; init; }
        public string Link { get; set; }
        public DateTime CreationTimeUtc { get; init; }
        public DateTime? DeadlineTimeUtc { get; init; }

        public int AuthorId { get; init; }
        public virtual IwentysUser Author { get; init; }

        public int? SubjectId { get; init; }
        public virtual Subject Subject { get; init; }

        public virtual ICollection<StudentAssignment> StudentAssignments { get; init; }

        public static Assignment Create(IwentysUser author, AssignmentCreateArguments createArguments)
        {
            return new Assignment
            {
                Title = createArguments.Title,
                Description = createArguments.Description,
                CreationTimeUtc = DateTime.UtcNow,
                DeadlineTimeUtc = createArguments.DeadlineTimeUtc,
                AuthorId = author.Id,
                SubjectId = createArguments.SubjectId
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