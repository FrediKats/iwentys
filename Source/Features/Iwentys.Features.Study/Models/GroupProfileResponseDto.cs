using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public record GroupProfileResponseDto(
        int Id,
        string GroupName,
        List<StudentInfoDto> Students,
        List<SubjectEntity> Subjects)
    {
        public GroupProfileResponseDto(StudyGroupEntity group)
            : this(
                group.Id,
                group.GroupName,
                group.Students.SelectToList(s => new StudentInfoDto(s)),
                group.GroupSubjects.SelectToList(gs => gs.Subject))
        {
        }
    }
}