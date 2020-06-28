using System;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Implementations
{
    public class TournamentService : ITournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Tournament[] Get()
        {
            return _tournamentRepository.Read();
        }

        public Tournament[] GetActive()
        {
            return _tournamentRepository
                .Read()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .ToArray();
        }

        public Tournament Get(int tournamentId)
        {
            return _tournamentRepository.ReadById(tournamentId);
        }
    }
}