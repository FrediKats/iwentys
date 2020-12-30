using System.Collections.Generic;
using System.Linq;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Assignments.Entities
{
    public class StudentAssignment
    {
        public int AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }

        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        public static StudentAssignment Create(Student creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var assignmentEntity = Assignment.Create(creator, assignmentCreateRequestDto);
            var studentAssignmentEntity = new StudentAssignment
            {
                StudentId = creator.Id,
                Assignment = assignmentEntity
            };

            return studentAssignmentEntity;
        }

        public static List<StudentAssignment> CreateForGroup(GroupAdminUser groupAdmin, AssignmentCreateRequestDto assignmentCreateRequestDto, StudyGroup studyGroup)
        {
            var assignmentEntity = Assignment.Create(groupAdmin.Student, assignmentCreateRequestDto);
            
            List<StudentAssignment> studentAssignmentEntities = studyGroup.Students.Select(s => new StudentAssignment
            {
                StudentId = s.Id,
                Assignment = assignmentEntity
            }).ToList();

            return studentAssignmentEntities;
        }
    }
}