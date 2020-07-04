using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildService
    {
        GuildProfileDto Create(AuthorizedUser creator, GuildCreateArgumentDto arguments);
        GuildProfileDto Update(AuthorizedUser user, GuildUpdateArgumentDto arguments);
        GuildProfileDto ApproveGuildCreating(AuthorizedUser user, int guildId);

        GuildProfileDto[] Get();
        GuildProfileDto Get(int id);
        GuildProfileDto GetStudentGuild(int userId);

        //TODO: customize dto for different voting
        VotingInfoDto StartVotingForLeader(AuthorizedUser creator, int guildId, GuildLeaderVotingCreateDto votingCreateDto);
        VotingInfoDto StartVotingForTotem(AuthorizedUser creator, int guildId, GuildTotemVotingCreateDto votingCreateDto);
        void SetTotem(AuthorizedUser user, int guildId, int totemId);

        Tribute SendTribute(AuthorizedUser user, int guildId, int projectId);
        Tribute CancelTribute(AuthorizedUser user, int tributeId);
        Tribute CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto);
    }
}