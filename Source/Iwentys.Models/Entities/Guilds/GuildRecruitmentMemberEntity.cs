namespace Iwentys.Models.Entities.Guilds
{
    public class GuildRecruitmentMemberEntity
    {
        public int GuildRecruitmentId { get; set; }
        public GuildRecruitmentEntity GuildRecruitment { get; set; }

        public int MemberId { get; set; }
        public StudentEntity Member { get; set; }
    }
}