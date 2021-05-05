using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds
{
    public class GuildRecruitmentMember
    {
        public int GuildRecruitmentId { get; init; }
        public virtual GuildRecruitment GuildRecruitment { get; init; }

        public int MemberId { get; init; }
        public virtual IwentysUser Member { get; init; }
    }
}