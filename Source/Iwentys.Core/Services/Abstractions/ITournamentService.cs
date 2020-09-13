using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ITournamentService
    {
        TournamentEntity[] Get();
        TournamentEntity[] GetActive();

        TournamentEntity Get(int tournamentId);
        TournamentLeaderboardDto GetLeaderboard(int tournamentId);
    }
}