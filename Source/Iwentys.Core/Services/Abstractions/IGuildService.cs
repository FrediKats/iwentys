using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Github;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildService
    {
        GuildProfileShortInfoDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments);
        GuildProfileShortInfoDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments);
        GuildProfileShortInfoDto ApproveGuildCreating(AuthorizedUser user, int guildId);

        GuildProfileDto[] Get();
        GuildProfileDto Get(int id, int? userId);
        GuildProfileDto GetStudentGuild(int userId);

        GuildProfileDto EnterGuild(AuthorizedUser user, int guildId);
        GuildProfileDto RequestGuild(AuthorizedUser user, int guildId);
        GuildProfileDto LeaveGuild(AuthorizedUser user, int guildId);

        VotingInfoDto StartVotingForLeader(AuthorizedUser creator, int guildId, GuildLeaderVotingCreateDto votingCreateDto);
        VotingInfoDto StartVotingForTotem(AuthorizedUser creator, int guildId, GuildTotemVotingCreateDto votingCreateDto);
        void SetTotem(AuthorizedUser user, int guildId, int totemId);

        Tribute[] GetPendingTributes(AuthorizedUser user);
        Tribute[] GetStudentTributeResult(AuthorizedUser user);
        Tribute CreateTribute(AuthorizedUser user, int projectId);
        Tribute CancelTribute(AuthorizedUser user, int tributeId);
        Tribute CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto);

        GithubRepository AddPinnedRepository(AuthorizedUser user, int guildId, string repositoryUrl);
        GithubRepository DeletePinnedRepository(AuthorizedUser user, int guildId, string repositoryUrl);
    }
}