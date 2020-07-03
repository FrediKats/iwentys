using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;

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

        void SendTribute(AuthorizedUser user, int guildId, int projectId);
    }
}