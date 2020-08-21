using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Github;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildService
    {
        GuildProfileShortInfoDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments);
        GuildProfileShortInfoDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments);
        GuildProfileShortInfoDto ApproveGuildCreating(AuthorizedUser user, int guildId);

        GuildProfileDto[] Get();
        GuildProfilePreviewDto[] GetOverview(int skippedCount, int takenCount);
        GuildProfileDto Get(int id, int? userId);
        GuildProfileDto GetStudentGuild(int userId);

        GuildProfileDto EnterGuild(AuthorizedUser user, int guildId);
        GuildProfileDto RequestGuild(AuthorizedUser user, int guildId);
        GuildProfileDto LeaveGuild(AuthorizedUser user, int guildId);

        GuildMember[] GetGuildRequests(AuthorizedUser user, int guildId);
        GuildMember[] GetGuildBlocked(AuthorizedUser user, int guildId);

        void BlockGuildMember(AuthorizedUser user, int guildId, int memberId);
        void UnblockStudent(AuthorizedUser user, int guildId, int studentId);
        void KickGuildMember(AuthorizedUser user, int guildId, int memberId);
        void AcceptRequest(AuthorizedUser user, int guildId, int studentId);
        void RejectRequest(AuthorizedUser user, int guildId, int studentId);

        VotingInfoDto StartVotingForLeader(AuthorizedUser creator, int guildId, GuildLeaderVotingCreateDto votingCreateDto);

        GithubRepository AddPinnedRepository(AuthorizedUser user, int guildId, string owner, string projectName);
        void UnpinProject(AuthorizedUser user, int pinnedProjectId);
    }
}