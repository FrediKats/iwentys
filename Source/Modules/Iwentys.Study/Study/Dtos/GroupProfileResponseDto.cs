using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.AccountManagement;
using Iwentys.Domain.Study;

namespace Iwentys.Study
{
    public record GroupProfileResponseDto
    {
        public int Id { get; init; }
        public string GroupName { get; init; }
        public int? GroupAdminId { get; set; }
        public List<StudentInfoDto> Students { get; set; }
        public List<SubjectProfileDto> Subjects { get; init; }

        public StudentInfoDto GroupAdmin => GroupAdminId is null ? null : Students.Find(s => s.Id == GroupAdminId);

        public GroupProfileResponseDto()
        {
        }
        public GroupProfileResponseDto(StudyGroup entity)
        {
            Id = entity.Id;
            GroupName = entity.GroupName;
            GroupAdminId = entity.GroupAdminId;
            Students = entity.Students.Select(s => new StudentInfoDto(s)).ToList();
            Subjects = entity.GroupSubjects.Select(gs => new SubjectProfileDto(gs.Subject)).ToList();
        }

        public static Expression<Func<StudyGroup, GroupProfileResponseDto>> FromEntity =>
            entity => new GroupProfileResponseDto
            {
                Id = entity.Id,
                GroupName = entity.GroupName,
                GroupAdminId = entity.GroupAdminId,
                Students = entity.Students.Select(s => new StudentInfoDto(s)).ToList(),
                Subjects = entity.GroupSubjects.Select(gs => new SubjectProfileDto(gs.Subject)).ToList()
            };
    }
}