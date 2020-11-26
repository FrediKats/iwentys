using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Models.Transferable.Study
{
    public class GroupProfileResponse
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

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