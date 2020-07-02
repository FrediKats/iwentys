using System;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services.Implementations
{
    public class TournamentService : ITournamentService
    {
        private readonly IGithubApiAccessor _githubApi;
        private readonly IGuildService _guildService;
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository, IGuildService guildService, IGithubApiAccessor githubApi)
        {
            _tournamentRepository = tournamentRepository;
            _guildService = guildService;
            _githubApi = githubApi;
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

        public TournamentLeaderboardDto GetLeaderboard(int tournamentId)
        {
            return Get(tournamentId)
                .WrapToDomain(_guildService, _githubApi)
                .GetLeaderboard();
        }
    }
}