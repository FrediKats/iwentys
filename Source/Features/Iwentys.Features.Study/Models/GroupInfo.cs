using Iwentys.Features.Study.Entities;

namespace Iwentys.Features.Study.Models
{
    public class GroupInfo
    {
        public int Id { get; set; }
        public string GroupName { get; set; }

        public static GroupInfo Wrap(StudyGroupEntity group)
        {
            return new GroupInfo
            {
                Id = group.Id,
                GroupName = group.GroupName,
            };
        }
    }
}