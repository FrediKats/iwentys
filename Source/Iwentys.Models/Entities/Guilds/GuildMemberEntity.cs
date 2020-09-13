using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class GuildMemberEntity
    {
        public int GuildId { get; set; }
        public GuildEntity Guild { get; set; }

        public int MemberId { get; set; }
        public Student Member { get; set; }

        public GuildMemberType MemberType { get; set; }

        public GuildMemberEntity()
        {
        }

        public GuildMemberEntity(GuildEntity guild, Student student, GuildMemberType memberType) : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMemberEntity(int guildId, int studentId, GuildMemberType memberType)
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }
    }
}