using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.GithubIntegration.Models;
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
        private readonly GithubIntegrationService _githubIntegrationService;

        private readonly Tournament _tournament;

        private readonly IGenericRepository<TournamentTeamMember> _tournamentTeamMemberRepository;
        private readonly IUnitOfWork _unitOfWork;

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
                throw InnerLogicException.TournamentException.IsNotFinished(_tournament.Id);

            TournamentParticipantTeam winner = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .OrderByDescending(t => t.Members.Sum(m => m.Points))
                .FirstOrDefaultAsync();

            if (winner is null)
                throw InnerLogicException.TournamentException.NoTeamRegistered(_tournament.Id);

            await _achievementProvider.AchieveForGuild(AchievementList.Tournaments.TournamentWinner, winner.GuildId);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateResult()
        {
            if (!_tournament.IsActive)
                throw InnerLogicException.TournamentException.AlreadyFinished(_tournament.Id);

            List<TournamentTeamMember> members = await _unitOfWork.GetRepository<TournamentParticipantTeam>()
                .Get()
                .Where(team => team.TournamentId == _tournament.Id)
                .SelectMany(team => team.Members)
                .Include(m => m.Member)
                .ToListAsync();

            foreach (TournamentTeamMember member in members)
            {
                ContributionFullInfo contributionFullInfo = await _githubIntegrationService.User.FindUserContributionOrEmpty(member.Member);
                member.Points = contributionFullInfo.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime);
            }

            _tournamentTeamMemberRepository.Update(members);
            await _unitOfWork.CommitAsync();
        }

        private int CountGuildRating(Guild guild)
        {
            var domain = new GuildDomain(guild,
                _githubIntegrationService,
                _unitOfWork.GetRepository<IwentysUser>(),
                _unitOfWork.GetRepository<GuildMember>(),
                _unitOfWork.GetRepository<GuildLastLeave>());
            //TODO: remove result
            List<GuildMemberImpactDto> users = domain.GetMemberImpacts().Result;

            return users
                .Select(userData => userData.Contribution.GetActivityForPeriod(_tournament.StartTime, _tournament.EndTime))
                .Sum();
        }
    }
}