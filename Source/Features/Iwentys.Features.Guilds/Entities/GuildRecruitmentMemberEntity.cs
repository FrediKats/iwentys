using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitmentMemberEntity
    {
        public int GuildRecruitmentId { get; set; }
        public virtual GuildRecruitmentEntity GuildRecruitment { get; set; }

        public int MemberId { get; set; }
        public virtual StudentEntity Member { get; set; }
    }
}