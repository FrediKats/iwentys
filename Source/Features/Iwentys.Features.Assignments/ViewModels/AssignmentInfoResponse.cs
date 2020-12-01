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
        public StudentPartialProfileDto Creator { get; set; }
        public SubjectEntity Subject { get; set; }
        public bool IsCompeted { get; set; }

        public static AssignmentInfoResponse Wrap(StudentAssignmentEntity studentAssignment)
        {
            return new AssignmentInfoResponse
            {
                Id = studentAssignment.AssignmentId,
                Title = studentAssignment.Assignment.Title,
                Description = studentAssignment.Assignment.Description,
                CreationTime = studentAssignment.Assignment.CreationTime,
                Creator = new StudentPartialProfileDto(studentAssignment.Assignment.Creator),
                Subject = studentAssignment.Assignment.Subject,
                IsCompeted = studentAssignment.IsCompeted
            };
        }
    }
}