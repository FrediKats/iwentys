using System.Collections.Generic;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitment
    {
        public int Id { get; init; }

        public int GuildId { get; init; }
        public virtual Guild Guild { get; init; }

        public string Description { get; init; }
        public virtual List<GuildRecruitmentMember> RecruitmentMembers { get; init; }
    }
}