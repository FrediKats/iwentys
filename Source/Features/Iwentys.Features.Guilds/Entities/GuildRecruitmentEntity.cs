using System.Collections.Generic;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildRecruitmentEntity
    {
        public int Id { get; set; }

        public int GuildId { get; set; }
        public GuildEntity Guild { get; set; }

        public string Description { get; set; }
        public List<GuildRecruitmentMemberEntity> RecruitmentMembers { get; set; }
    }
}