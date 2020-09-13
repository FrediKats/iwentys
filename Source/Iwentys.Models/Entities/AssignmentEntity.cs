using System;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable;

namespace Iwentys.Models.Entities
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

        public static AssignmentEntity Create(StudentEntity creator, AssignmentCreateDto assignmentCreateDto)
        {
            return new AssignmentEntity
            {
                Title = assignmentCreateDto.Title,
                Description = assignmentCreateDto.Description,
                CreationTime = DateTime.UtcNow,
                CreatorId = creator.Id,
                SubjectId = assignmentCreateDto.SubjectId
            };
        }
    }
}