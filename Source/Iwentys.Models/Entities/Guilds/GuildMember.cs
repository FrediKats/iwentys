using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class GuildMember
    {
        public int GuildId { get; set; }
        public GuildEntity Guild { get; set; }

        public int MemberId { get; set; }
        public Student Member { get; set; }

        public GuildMemberType MemberType { get; set; }

        public static GuildMember NewMember(int guildId, int memberId)
        {
            return new GuildMember
            {
                GuildId = guildId,
                MemberId = memberId,
                MemberType = GuildMemberType.Member
            };
        }

        public static GuildMember NewMentor(int guildId, int memberId)
        {
            return new GuildMember
            {
                GuildId = guildId,
                MemberId = memberId,
                MemberType = GuildMemberType.Mentor
            };
        }

        public static GuildMember NewRequest(int guildId, int memberId)
        {
            return new GuildMember
            {
                GuildId = guildId,
                MemberId = memberId,
                MemberType = GuildMemberType.Requested
            };
        }
    }
}