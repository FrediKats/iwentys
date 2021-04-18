using System.Threading.Tasks;
using Iwentys.Domain.Models;

namespace Iwentys.Domain.Guilds
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
        void RewardWinners();
        void UpdateResult();
    }
}