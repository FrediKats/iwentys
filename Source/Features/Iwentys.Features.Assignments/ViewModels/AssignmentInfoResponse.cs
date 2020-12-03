using System;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Features.Assignments.ViewModels
{
    public class AssignmentInfoResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public StudentPartialProfileDto Creator { get; set; }
        public SubjectEntity Subject { get; set; }
        public bool IsCompeted { get; set; }

        public static AssignmentInfoResponse Wrap(StudentAssignmentEntity studentAssignment)
        {
            return Wrap(studentAssignment.Assignment);
        }

        public static AssignmentInfoResponse Wrap(AssignmentEntity assignment)
        {
            return new AssignmentInfoResponse
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                CreationTime = assignment.CreationTime,
                Deadline = assignment.Deadline,
                Creator = new StudentPartialProfileDto(assignment.Creator),
                Subject = assignment.Subject,
                IsCompeted = assignment.IsCompleted
            };
        }
    }
}