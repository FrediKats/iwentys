using System.Threading.Tasks;
using Iwentys.Domain.Models;

namespace Iwentys.Domain.Guilds
{
    public interface ITournamentDomain
    {
        TournamentLeaderboardDto GetLeaderboard();
        Task RewardWinners();
        Task UpdateResult();
    }
}