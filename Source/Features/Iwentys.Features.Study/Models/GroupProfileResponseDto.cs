using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Enums;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Features.Study.Models
{
    public record GroupProfileResponseDto
    {
        public int Id { get; init; }
        public string GroupName { get; init; }
        public List<StudentInfoDto> Students { get; set; }
        public List<Subject> Subjects { get; init; }

        public GroupProfileResponseDto(StudyGroup group)
            : this(
                group.Id,
                group.GroupName,
                group.Students.Select(s => new StudentInfoDto(s)).ToList(),
                group.GroupSubjects.Select(gs => gs.Subject).ToList())
        {
        }

        public GroupProfileResponseDto(int id, string groupName, List<StudentInfoDto> students, List<Subject> subjects) :this()
        {
            Id = id;
            GroupName = groupName;
            Students = students;
            Subjects = subjects;
        }

        public GroupProfileResponseDto()
        {
        }

        public static Expression<Func<StudyGroup, GroupProfileResponseDto>> FromEntity =>
            entity => new GroupProfileResponseDto(entity);

        public StudentInfoDto GroupAdmin => Students.FirstOrDefault(s => s.Role == StudentRole.GroupAdmin);
    }
}