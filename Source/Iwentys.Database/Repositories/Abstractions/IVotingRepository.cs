using Iwentys.Models.Entities.Votes;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IVotingRepository
    {
        Voting GetVoting(int votingId);
    }
}