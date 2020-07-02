using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.Voting;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildService
    {
        GuildProfileDto Create(int creatorId, GuildCreateArgumentDto arguments);
        GuildProfileDto Update(int creator, GuildUpdateArgumentDto arguments);
        GuildProfileDto ApproveGuildCreating(int adminId, int guildId);

        GuildProfileDto[] Get();
        GuildProfileDto Get(int id);
        GuildProfileDto GetUserGuild(int userId);

        //TODO: customize dto for different voting
        VotingInfoDto StartVotingForLeader(int initiatorId, int guildId, VotingCreateDto votingCreateDto);
        VotingInfoDto StartVotingForTotem(int initiatorId, int guildId, VotingCreateDto votingCreateDto);

    }
}