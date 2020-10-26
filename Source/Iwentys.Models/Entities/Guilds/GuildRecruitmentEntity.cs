using System.Collections.Generic;

namespace Iwentys.Models.Entities.Guilds
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