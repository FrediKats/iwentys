using System.Collections.Generic;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitment
    {
        public int Id { get; set; }

        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }

        public string Description { get; set; }
        public virtual List<GuildRecruitmentMember> RecruitmentMembers { get; set; }
    }
}