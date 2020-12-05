using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public class GroupProfileResponse : GroupInfo
    {
        public List<StudentPartialProfileDto> Students { get; set; }
        public List<SubjectEntity> Subjects { get; set; }

        public static GroupProfileResponse Create(StudyGroupEntity group)
        {
            return new GroupProfileResponse
            {
                Id = group.Id,
                GroupName = group.GroupName,
                Students = group.Students.SelectToList(s => new StudentPartialProfileDto(s)),
                Subjects = group.GroupSubjects.SelectToList(gs => gs.Subject)
            };
        }
    }
}