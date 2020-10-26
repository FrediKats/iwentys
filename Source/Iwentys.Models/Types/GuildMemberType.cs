using System.Linq;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Models.Types
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
        public static bool IsMember(this GuildMemberType guildMemberType)
        {
            return guildMemberType == GuildMemberType.Creator ||
                   guildMemberType == GuildMemberType.Mentor ||
                   guildMemberType == GuildMemberType.Member;
        }

        public static IQueryable<GuildMemberEntity> WhereIsMember(this IQueryable<GuildMemberEntity> queryable)
        {
            return queryable.Where(gm => gm.MemberType == GuildMemberType.Creator ||
                                         gm.MemberType == GuildMemberType.Mentor ||
                                         gm.MemberType == GuildMemberType.Member);
        }

        public static IQueryable<GuildMemberEntity> WhereIsEditor(this IQueryable<GuildMemberEntity> queryable)
        {
            return queryable.Where(gm => gm.MemberType == GuildMemberType.Creator ||
                                         gm.MemberType == GuildMemberType.Mentor);
        }

        public static bool IsEditor(this GuildMemberType guildMemberType)
        {
            return guildMemberType == GuildMemberType.Creator ||
                   guildMemberType == GuildMemberType.Mentor;
        }
    }
}