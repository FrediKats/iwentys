using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record GroupProfileResponseDto
    {
        public GroupProfileResponseDto(StudyGroupEntity group)
            : this(
                group.Id,
                group.GroupName,
                group.Students.SelectToList(s => new StudentInfoDto(s)),
                group.GroupSubjects.SelectToList(gs => gs.Subject))
        {
        }

        public GroupProfileResponseDto(int id, string groupName, List<StudentInfoDto> students, List<SubjectEntity> subjects) :this()
        {
            Id = id;
            GroupName = groupName;
            Students = students;
            Subjects = subjects;
        }

        public GroupProfileResponseDto()
        {
        }

        public int Id { get; init; }
        public string GroupName { get; init; }
        public List<StudentInfoDto> Students { get; init; }
        public List<SubjectEntity> Subjects { get; init; }
    }
}