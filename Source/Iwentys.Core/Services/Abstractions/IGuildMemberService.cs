using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildMemberService
    {
        GuildProfileDto EnterGuild(AuthorizedUser user, int guildId);
        GuildProfileDto RequestGuild(AuthorizedUser user, int guildId);
        GuildProfileDto LeaveGuild(AuthorizedUser user, int guildId);

        GuildMemberEntity[] GetGuildRequests(AuthorizedUser user, int guildId);
        GuildMemberEntity[] GetGuildBlocked(AuthorizedUser user, int guildId);

        void BlockGuildMember(AuthorizedUser user, int guildId, int memberId);
        void UnblockStudent(AuthorizedUser user, int guildId, int studentId);
        void KickGuildMember(AuthorizedUser user, int guildId, int memberId);
        void AcceptRequest(AuthorizedUser user, int guildId, int studentId);
        void RejectRequest(AuthorizedUser user, int guildId, int studentId);
    }
}