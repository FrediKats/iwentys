using Iwentys.Models.Transferable.Voting;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IVotingService
    {
        VotingInfoDto CreateGuildVoting(VotingCreateDto votingCreateDto);

        VotingInfoDto Get(int id);
        VotingInfoDto[] GetGuildVoting(int guildId);

        void Vote(int votingId, int userId, int answerId);
    }
}