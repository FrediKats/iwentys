using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitmentMember
    {
        public int GuildRecruitmentId { get; init; }
        public virtual GuildRecruitment GuildRecruitment { get; init; }

        public int MemberId { get; init; }
        public virtual Student Member { get; init; }
    }
}