using System;

namespace Iwentys.Models.Types.Guilds
{
    public enum GuildMemberType
    {
        Requested = 1,
        Member = 2,
        Mentor = 3,
        Creator = 4,
        Blocked = 5
    }

    public static class GuildMemberTypeExtension
    {
        public static Boolean IsMember(this GuildMemberType guildMemberType)
        {
            return guildMemberType == GuildMemberType.Creator ||
                   guildMemberType == GuildMemberType.Mentor || 
                   guildMemberType == GuildMemberType.Member;
        }

        public static Boolean IsEditor(this GuildMemberType guildMemberType)
        {
            return guildMemberType == GuildMemberType.Creator || 
                   guildMemberType == GuildMemberType.Mentor;
        }
    }
}