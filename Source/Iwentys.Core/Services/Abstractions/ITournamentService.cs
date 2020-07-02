using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ITournamentService
    {
        Tournament[] Get();
        Tournament[] GetActive();

        Tournament Get(int tournamentId);
        TournamentLeaderboardDto GetLeaderboard(int tournamentId);
    }
}