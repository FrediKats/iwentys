using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Abstractions
{
    public interface ITournamentService
    {
        Tournament[] Get();
        Tournament[] GetActive();

        Tournament Get(int tournamentId);
    }
}