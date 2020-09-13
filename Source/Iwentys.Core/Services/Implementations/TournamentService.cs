using System;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.GithubIntegration;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Transferable.Tournaments;

namespace Iwentys.Core.Services.Implementations
{
    public class TournamentService : ITournamentService
    {
        private readonly DatabaseAccessor _databaseAccessor;
        private readonly IGithubApiAccessor _githubApi;
        private readonly IGithubUserDataService _githubUserDataService;

        public TournamentService(DatabaseAccessor databaseAccessor, IGithubApiAccessor githubApi, IGithubUserDataService githubUserDataService)
        {
            _githubApi = githubApi;
            _databaseAccessor = databaseAccessor;
            _githubUserDataService = githubUserDataService;
        }

        public TournamentEntity[] Get()
        {
            return _databaseAccessor.Tournament.Read().ToArray();
        }

        public TournamentEntity[] GetActive()
        {
            return _databaseAccessor.Tournament
                .Read()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .ToArray();
        }

        public TournamentEntity Get(int tournamentId)
        {
            return _databaseAccessor.Tournament.ReadById(tournamentId);
        }

        public TournamentLeaderboardDto GetLeaderboard(int tournamentId)
        {
            return Get(tournamentId)
                .WrapToDomain(_githubApi, _databaseAccessor, _githubUserDataService)
                .GetLeaderboard();
        }
    }
}