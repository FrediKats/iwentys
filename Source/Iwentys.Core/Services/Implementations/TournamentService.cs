using System;
using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Integrations.GithubIntegration;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable;
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

        public TournamentInfoResponse[] Get()
        {
            return _databaseAccessor.Tournament
                .Read()
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public TournamentInfoResponse[] GetActive()
        {
            return _databaseAccessor.Tournament
                .Read()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public TournamentInfoResponse Get(int tournamentId)
        {
            return _databaseAccessor.Tournament.ReadById(tournamentId).To(TournamentInfoResponse.Wrap);
        }

        public TournamentLeaderboardDto GetLeaderboard(int tournamentId)
        {
            return _databaseAccessor.Tournament
                .ReadById(tournamentId)
                .WrapToDomain(_githubApi, _databaseAccessor, _githubUserDataService)
                .GetLeaderboard();
        }
    }
}