using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Tournaments;
using Iwentys.Features.Guilds.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class TournamentService
    {
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IGithubApiAccessor _githubApi;
        private readonly GithubUserDataService _githubUserDataService;

        public TournamentService(ITournamentRepository tournamentRepository, IGithubApiAccessor githubApi, GithubUserDataService githubUserDataService)
        {
            _tournamentRepository = tournamentRepository;
            _githubApi = githubApi;
            _githubUserDataService = githubUserDataService;
        }

        public async Task<List<TournamentInfoResponse>> Get()
        {
            List<TournamentEntity> tournaments = await _tournamentRepository
                .Read()
                .ToListAsync();

            return tournaments.SelectToList(TournamentInfoResponse.Wrap);
        }

        public TournamentInfoResponse[] GetActive()
        {
            return _tournamentRepository
                .Read()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TournamentInfoResponse> Get(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.ReadByIdAsync(tournamentId);
            return TournamentInfoResponse.Wrap(tournamentEntity);
        }

        public async Task<TournamentLeaderboardDto> GetLeaderboard(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.ReadByIdAsync(tournamentId);
            return tournamentEntity
                .WrapToDomain(_githubApi, _githubUserDataService)
                .GetLeaderboard();
        }
    }
}