using System;
using Iwentys.Features.Assignments.ViewModels;
using Iwentys.Features.StudentFeature.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class AssignmentEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }

        public int CreatorId { get; set; }
        public StudentEntity Creator { get; set; }

        public int? SubjectId { get; set; }
        public SubjectEntity Subject { get; set; }

        public static AssignmentEntity Create(StudentEntity creator, AssignmentCreateRequest assignmentCreateRequest)
        {
            return new AssignmentEntity
            {
                Title = assignmentCreateRequest.Title,
                Description = assignmentCreateRequest.Description,
                CreationTime = DateTime.UtcNow,
                CreatorId = creator.Id,
                SubjectId = assignmentCreateRequest.SubjectId
            };
        }
    }
}