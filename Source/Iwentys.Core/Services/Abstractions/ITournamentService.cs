using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ITournamentService
    {
        TournamentInfoResponse[] Get();
        TournamentInfoResponse[] GetActive();

        TournamentInfoResponse Get(int tournamentId);
        TournamentLeaderboardDto GetLeaderboard(int tournamentId);
    }
}