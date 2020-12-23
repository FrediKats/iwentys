using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignmentEntity
    {
        public int AssignmentId { get; set; }
        public virtual AssignmentEntity Assignment { get; set; }

        public int StudentId { get; set; }
        public virtual StudentEntity Student { get; set; }

        public static StudentAssignmentEntity Create(StudentEntity creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var assignmentEntity = AssignmentEntity.Create(creator, assignmentCreateRequestDto);
            var studentAssignmentEntity = new StudentAssignmentEntity
            {
                StudentId = creator.Id,
                Assignment = assignmentEntity
            };

            return studentAssignmentEntity;
        }

        public static List<StudentAssignmentEntity> CreateForGroup(GroupAdminUser groupAdmin, AssignmentCreateRequestDto assignmentCreateRequestDto, StudyGroupEntity studyGroup)
        {
            var assignmentEntity = AssignmentEntity.Create(groupAdmin.Student, assignmentCreateRequestDto);
            
            List<StudentAssignmentEntity> studentAssignmentEntities = studyGroup.Students.Select(s => new StudentAssignmentEntity
            {
                StudentId = s.Id,
                Assignment = assignmentEntity
            }).ToList();

            return studentAssignmentEntities;
        }
    }
}