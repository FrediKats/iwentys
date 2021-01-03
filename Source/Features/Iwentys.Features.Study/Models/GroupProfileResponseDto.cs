using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Features.Study.Models
{
    public record GroupProfileResponseDto
    {
        public int Id { get; init; }
        public string GroupName { get; init; }
        public StudentInfoDto GroupAdmin { get; set; }
        public List<StudentInfoDto> Students { get; set; }
        public List<Subject> Subjects { get; init; }

        public GroupProfileResponseDto(StudyGroup group)
            : this(
                group.Id,
                group.GroupName,
                //TODO: OMG # _ # 
                group.GroupAdminId == null ? null : new StudentInfoDto(group.Students.First(s => s.StudentId == group.GroupAdminId.Value).Student),
                group.Students.Select(s => new StudentInfoDto(s.Student)).ToList(),
                group.GroupSubjects.Select(gs => gs.Subject).ToList())
        {
        }

        public GroupProfileResponseDto(int id, string groupName, StudentInfoDto groupAdmin, List<StudentInfoDto> students, List<Subject> subjects) :this()
        {
            Id = id;
            GroupName = groupName;
            GroupAdmin = groupAdmin;
            Students = students;
            Subjects = subjects;
        }

        public GroupProfileResponseDto()
        {
        }

        public static Expression<Func<StudyGroup, GroupProfileResponseDto>> FromEntity =>
            entity => new GroupProfileResponseDto(entity);
    }
}