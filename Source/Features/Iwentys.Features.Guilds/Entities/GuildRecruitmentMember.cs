using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitmentMember
    {
        public int GuildRecruitmentId { get; set; }
        public virtual GuildRecruitment GuildRecruitment { get; set; }

        public int MemberId { get; set; }
        public virtual Student Member { get; set; }
    }
}