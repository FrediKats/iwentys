using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Guilds;

public class GuildMentor
{
    public GuildMentor(IwentysUser user, Guild guild, GuildMemberType memberType)
    {
        if (!memberType.IsMentor())
            throw InnerLogicException.GuildExceptions.IsNotGuildMentor(user.Id);

        User = user;
        Guild = guild;
        MemberType = memberType;
    }

    public IwentysUser User { get; }
    public Guild Guild { get; }
    public GuildMemberType MemberType { get; }
}

public static class GuildMentorUserExtensions
{
    public static GuildMentor EnsureIsGuildMentor(this IwentysUser user, Guild guild)
    {
        GuildMember membership = guild.Members.FirstOrDefault(m => m.MemberId == user.Id);

        if (membership is null)
            throw InnerLogicException.GuildExceptions.IsNotGuildMember(user.Id, guild.Id);

        return new GuildMentor(user, guild, membership.MemberType);
    }

    public static async Task<GuildMentor> EnsureIsGuildMentor(this Task<IwentysUser> user, Guild guild)
    {
        IwentysUser member = await user;

        GuildMember membership = guild.Members.FirstOrDefault(m => m.MemberId == member.Id);

        if (membership is null)
            throw InnerLogicException.GuildExceptions.IsNotGuildMember(member.Id, guild.Id);

        return new GuildMentor(member, guild, membership.MemberType);
    }
}