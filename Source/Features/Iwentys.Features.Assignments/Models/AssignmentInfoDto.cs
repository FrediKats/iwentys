using System;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Models
{
    public class AssignmentInfoDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public StudentInfoDto Creator { get; set; }
        public SubjectEntity Subject { get; set; }
        public bool IsCompeted { get; set; }

        public static AssignmentInfoDto Wrap(StudentAssignmentEntity studentAssignment)
        {
            return Wrap(studentAssignment.Assignment);
        }

        public static AssignmentInfoDto Wrap(AssignmentEntity assignment)
        {
            return new AssignmentInfoDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CreationTime = assignment.CreationTime,
                Deadline = assignment.Deadline,
                Creator = new StudentInfoDto(assignment.Creator),
                Subject = assignment.Subject,
                IsCompeted = assignment.IsCompleted
            };
        }
    }
}