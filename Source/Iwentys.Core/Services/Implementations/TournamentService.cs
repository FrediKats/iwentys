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
        private readonly IGuildProfileService _guildProfileService;
        private readonly ITournamentRepository _tournamentRepository;

        public TournamentService(ITournamentRepository tournamentRepository, IGuildProfileService guildProfileService, IGithubApiAccessor githubApi)
        {
            _tournamentRepository = tournamentRepository;
            _guildProfileService = guildProfileService;
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
                .WrapToDomain(_guildProfileService, _githubApi)
                .GetLeaderboard();
        }
    }
}