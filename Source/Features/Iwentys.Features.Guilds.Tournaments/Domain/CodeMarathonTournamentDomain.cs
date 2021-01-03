using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.GithubIntegration.Services;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models;
using Iwentys.Features.Guilds.Tournaments.Entities;
using Iwentys.Features.Guilds.Tournaments.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Tournaments.Domain
{
    public class CodeMarathonTournamentDomain : ITournamentDomain
    {
        private readonly AchievementProvider _achievementProvider;

        private readonly Tournament _tournament;
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<TournamentTeamMember> _tournamentTeamMemberRepository;


        public CodeMarathonTournamentDomain(
            Tournament tournament,
            GithubIntegrationService githubIntegrationService,
            IUnitOfWork unitOfWork,
            AchievementProvider achievementProvider)
        {
            _tournament = tournament;
            _githubIntegrationService = githubIntegrationService;
            _unitOfWork = unitOfWork;
            _achievementProvider = achievementProvider;

            _tournamentTeamMemberRepository = _unitOfWork.GetRepository<TournamentTeamMember>();
        }

        public TournamentLeaderboardDto GetLeaderboard()
        {
            List<Guild> guilds = _unitOfWork
                .GetRepository<Guild>()
                .Get()
                .ToList();

            Dictionary<GuildProfileShortInfoDto, int> result = guilds
                .ToDictionary(g => new GuildProfileShortInfoDto(g), CountGuildRating);

            return new TournamentLeaderboardDto
            {
                Tournament = _tournament,
                Result = result
            };
        }

        public async Task RewardWinners()
        {
            if (_tournament.IsActive)
                throw new InnerLogicException("Tournament not finished");

            var winner = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .OrderByDescending(t => t.Members.Sum(m => m.Points))
                .FirstOrDefaultAsync();

            //TODO: it's not ok
            if (winner is null)
                throw new InnerLogicException("No team in tournament");

            await _achievementProvider.AchieveForGuild(AchievementList.Tournaments.TournamentWinner, winner.GuildId);
        }

        private int CountGuildRating(Guild guild)
        {
            var domain = new GuildDomain(guild, _githubIntegrationService, _unitOfWork.GetRepository<IwentysUser>(), _unitOfWork.GetRepository<GuildMember>());
            //TODO: remove result
            List<GuildMemberImpactDto> users = domain.GetMemberImpacts().Result;
            
            return users
                .Select(userData => userData.Contribution.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime))
                .Sum();
        }
        
        public async Task UpdateResult()
        {
            //TODO: skip with warning instead of exception?
            if (!_tournament.IsActive)
                throw new InnerLogicException("Tournament end already");

            List<TournamentTeamMember> members = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .SelectMany(team => team.Members)
                .Include(m => m.Member)
                .ToListAsync();

            foreach (TournamentTeamMember member in members)
            {
                var contributionFullInfo = await _githubIntegrationService.FindUserContributionOrEmpty(member.Member);
                member.Points = contributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime);
            }

            _tournamentTeamMemberRepository.Update(members);
            await _unitOfWork.CommitAsync();
        }
    }
}