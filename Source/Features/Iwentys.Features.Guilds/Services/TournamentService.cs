using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.GithubIntegration;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Tournaments;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Services
{
    public class TournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<TournamentEntity> _tournamentRepository;
        private readonly IGithubApiAccessor _githubApi;
        private readonly GithubIntegrationService _githubIntegrationService;

        public TournamentService(IGithubApiAccessor githubApi, GithubIntegrationService githubIntegrationService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _tournamentRepository = _unitOfWork.GetRepository<TournamentEntity>();
            _githubApi = githubApi;
            _githubIntegrationService = githubIntegrationService;
        }

        public async Task<List<TournamentInfoResponse>> GetAsync()
        {
            List<TournamentEntity> tournaments = await _tournamentRepository
                .GetAsync()
                .ToListAsync();

            return tournaments.SelectToList(TournamentInfoResponse.Wrap);
        }

        public TournamentInfoResponse[] GetActive()
        {
            return _tournamentRepository
                .GetAsync()
                .Where(t => t.StartTime < DateTime.UtcNow && t.EndTime > DateTime.UtcNow)
                .AsEnumerable()
                .Select(TournamentInfoResponse.Wrap)
                .ToArray();
        }

        public async Task<TournamentInfoResponse> GetAsync(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.GetByIdAsync(tournamentId);
            return TournamentInfoResponse.Wrap(tournamentEntity);
        }

        public async Task<TournamentLeaderboardDto> GetLeaderboard(int tournamentId)
        {
            TournamentEntity tournamentEntity = await _tournamentRepository.GetByIdAsync(tournamentId);
            return tournamentEntity
                .WrapToDomain(_githubApi, _githubIntegrationService)
                .GetLeaderboard();
        }
    }
}